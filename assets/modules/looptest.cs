// Testing speed behaviour of ElfScript using different methods
// FIXME add a docu somewhere
/*
 *  for loops
 *    I. The loop max (%i < max) variable test:
 *      1.) if possible use #define with a constant (ElfScript only)
 *      2.) if it's variable add a local variable before the loop
 *          like %count = %this.count to speed up
 *          also a global var is ok when not too many used
 *      3.) never use fields directly this is very bad!
 *      4.) never use a function
 *
 *   II. if's inside loop
 *      try not to set : for () ... if () {} else if
 *      if possible do if () for() ...if for()
 *
 *  III. set/get data: (details below) nothing i tried is really good.
 *      Best is using global Variables by reference. like:
 *      getVector2GlobalV / setVector2GlobalV
 *      and the using the global variable inside the loop.
 * */

//ElfScript extension via preprocessor:
#define _LOOP_COUNT_ 5000

// $testObj = new TestObj();

function createlooptest() {

    // using batch render to set positions
    %obj = new BatchRender(L) {
        class = "looptest";

        TypeS32 count = _LOOP_COUNT_;
        TypeS32 method = 17;
        TypeS32 methodCount = 20;

        TypeF32 lX = 0.0;
        TypeF32 lY = 0.0;
    };

     $gCount = %obj.count;
     %obj.dynCount = %obj.count;

    return %obj;
}
//----------------------------------------------------------------------
function looptest::onAdd(%this) {
    SetTargetFPS(0); //FPS TEST only calling module
    return true;
}
//----------------------------------------------------------------------
// function looptest::OnRemove(%this) {}
//----------------------------------------------------------------------
/*
 * FPS test on i5-7200U
 * L.methods:
 *  0: no loop at all        ~ 2600
 *  1: only loop 5k and the if (%this.method == 2)...
 *      - NOTE loop using field:  for ( %i = 0 ; %i < %this.count; %i++) ===> ~500fps
 *      - NOTE loop using local var: %count = %this.count; for ( %i = 0 ; %i < %count; %i++) ===> ~1000fps
 *      - NOTE using number for ( %i = 0 ; %i < 5000; %i++)  ==> 2000k fps
 *      - NOTE using global: for ( %i = 0 ; %i < $count; %i++) { => also ~1000fps
 *      - NOTE using defined const: _LOOP_COUNT_
 *
 *  ** using _LOOP_COUNT_ ** base ~ 2000 fps + local var
 *  2..5 funny the if/else eat a lot every if is empty in this test:
 *      2: => 1000
 *      3: => 1000
 *      4: => 700 !!!
 *      5: => 600 !!!
 *
 *  2..5 same but without else:
 *      all about 500 - 600
 *
 *  2..5 *if for* capsulate:
 *      all about 2k!!!! thats good to know !
 *
 *  ---- handling data from batchrender:
 *  2: setting static values via setVector2 => 260
 *  3. getVector2 into a local variable and split into to more =>  150
 *  4. using getX and getY into local variables => 100
 *  5. seting local variable values into setVector2 => 240
 *  6. setting into a global variable by static reference => 200
 *  7. getting from a global variable by static reference => 200
 *  8. setting into a global variable by dynamic reference => 100
 *  9. getting from a global variable by dynamic reference => 100
 * 10: setVector2v from a static string (like 2)=> 200
 * 11: setVector2v from local variables (like 5)=> 230
 *  NOTE: very interesting %tmpX SPC %tmpY is faster than static "11 12"
 *        with using local variales it makes not difference if using
 *        setVector2 or setVector2V the TAB is fast!
 * --- without batchrender:
 * 12: setting a 2 global fake array vars with static data => 180
 * 13: setting a 2 global fake array vars with local data => 170
 * 14: incrementing/decrementing 2 global fake array variables => 170
 *
 */

// $TEST_TEXT = ""
//----------------------------------------------------------------------
function looptest::Render(%this) {
     // ---------- display and change current method ------------
    ClearBackGround("60 60 60");
    DrawText("Method" SPC %this.method SPC _LOOP_COUNT_ SPC GetFPS(), 10 , 10, 20, YELLOW);
    DrawText("TAB/shift TAB to toggle", 10 , 40, 20,  GREEN);
    DrawText("Description in source files modules/looptest.cs", 10 , 70,20, "100 100 255");
    if (IsKeyPressed(KEY_TAB)) {
        if (isKeyDown(KEY_LEFT_SHIFT)) %this.method --;
        else %this.method ++;
        if ( %this.method < 0 )  %this.method = %this.methodCount;
        else if (%this.method >= %this.methodCount) %this.method = 0;
    }

    if ( $peep ) {

        %this.lx = "HUHU";
        $peep = false;
        echo("PEEP lx" SPC %this.lx SPC "STRICT=>" SPC (%this.lx $= "0"));
    }
    // ---------- loop method stuff ------------

    %m = %this.method; // NOTE allways to local!

    if(%m == 0) return; //no loop at all
    else
    if(%m == 1) {
        // %this.count static field ==> 550
        // %this.dynCount ==> 550
        // %lCount = %this.count; ==> 1000
        // $gCount ==> 1000
        // _LOOP_COUNT_ ==> 2000

        %lCount = %this.count;

        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {}
        return;
    }
    else
    if(%m == 2) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            %this.setVector2(%i,11,12);
            //crash !! BatchRender::setVector2(%this,%i,11,12);
            // maybe this did work with old ConsoleMethod ?!
            // but i guess it's not faster anyway
        }
    }
    else
    if(%m == 3) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
           %tmp = %this.getVector2(%i);
           // not fair ? i guess the assign it not required!
           // %tmpX = %tmp.x;
           // %tmpY = %tmp.y;
        }
    }
    else
    if(%m == 4) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            %tmpX = %this.getX(%i);
            %tmpY = %this.getY(%i);
        }
    }
    else
    if(%m == 5) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
             %this.setVector2(%i,%tmpX,%tmpY);
        }
    }
    else
    // to global
    if(%m == 6) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            %this.getVector2GlobalV(%i,"$gX","$gY");
            // %tmpX = $gX; // eat a lot
            // %tmpY = $gY;
        }
    }
    else
    // from global
    if(%m == 7) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
             %this.setVector2GlobalV(%i,"$gX","$gY");
        }
    }
    else
    if(%m == 8) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            %this.getVector2GlobalV(%i,"$gX" @ %i ,"$gY" @ %i );
        }
    }
    else
    if(%m == 9) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            %this.setVector2GlobalV(%i,"$gX" @ %i ,"$gY" @ %i);
        }
    }
    else
    if(%m == 10) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            %this.setVector2v(%i, "10 11");
        }
    }
    else
    if(%m == 11) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            %this.setVector2v(%i,%tmpX SPC %tmpY);
        }
    }
    else
    if(%m == 12) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            $VEC2x[%i] = 12;
            $VEC2y[%i] = 12;
        }
    }
    else
    if(%m == 13) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            $VEC2x[%i] = %tmpX;
            $VEC2y[%i] = %tmpY;
        }
    }
    else
    if(%m == 14) {
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            $VEC2x[%i]++;
            $VEC2y[%i]--;
        }
    }
    else
    if(%m == 15) {
        %foo = 0.0;
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            %foo += 0.1;
            %bar -= 0.3;
        }
    }
    else
    if(%m == 16) { // curelement stuff => pull
        %foo = 0.0;
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            // %this.pull(%i);
            %x = %this.x;
            %y = %this.y;
        }
    }
    else
    if(%m == 17) { // curelement stuff => push
        // i dont get why it works in my test script in
        // ElfScript demo code and not here ?!
        // %testObj = $testObj;
        %foo = 0.1;
        for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
            // OP = 10.1 is a string for the parser :(
            // OP = variable can be a float
            // %this.x = 20.1; // 500
            %this.x = %foo;
            // %this.y *= 0.1;
            // %this.push(%i);

             // %testObj.testFloat += 0.1;
        }
    }
    else
        if(%m == 18) { //  typed def

            for ( %i = 0 ; %i < _LOOP_COUNT_ ; %i++) {
                %this.lX = 20.1;
                %this.lY = 10.2;
            }
        }
    // BAD! use "if for" capsulate if possible
    // for ( %i = 0 ; %i < _LOOP_COUNT_; %i++) {
    //     if (%m == 2) {
    //         // %this.setVector2(%i,11,12);
    //
    //     }
    //     if (%m == 3) {
    //
    //     }
    //     if (%m == 4) {
    //
    //     }
    //
    //     if (%m == 5) {
    //     }
    // }


}

