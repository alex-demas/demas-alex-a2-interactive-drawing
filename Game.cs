// Include the namespaces (code libraries) you need below.
using System;
using System.Net.Security;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    /// <summary>
    ///     Your game code goes inside this class!
    /// </summary>
    public class Game
    {
        /// <summary>
        ///     Setup runs once before the game loop begins.
        /// </summary>
        /// 

        // -------------------- [ Variables ]-----------------------------------------------------------------------------

       // COLORS
        Color headColor = new Color("#29ABE2");
        Color bodyColor = new Color("#29ABE2");
        Color outlineColor = new Color("#0070BC");
        Color backgroundColor = new Color("#AEEBFD");
        Color backgroundBlock1 = new Color("#B9FFFF");
        Color backgroundBlock2 = new Color("#EBFFFF");
        Color cookieColor = new Color("#EEAD51");
        Color chipColor = new Color("#743212");
        // VARIABLES
        bool cookiedEaten = false;
        bool mouthOpen = false;
        bool blinkingEnabled = true;
        Vector2 headPosition = new Vector2(200, 300);
        Vector2 bodyPosition = new Vector2(200, 325);
        float transitionScreenTime = 0;
        float blinkTime = 0;
        float blinkCd = 0; // Cooldown

        // CONFIGS
        int OUTLINE_THICKNESS = 7; // Outline thickness
        int OPEN_DISTANCE = 100; // How close the mouse has to be to the mouth to open
        Vector2 HEAD_HOME_POSITION = new Vector2(200, 300); // Default position of the head
        Vector2 BODY_HOME_POSITION = new Vector2(200, 325); // Default position of the body
        float TRANSITION_SCREEN_TIME = 0.5f; // Black screen transition time
        float BLINK_TIME = 0.1f;
        float BLINK_MAX = 2f; // Blink max time
        float BLINK_MIN = 0.5f; // Blink min time


        // -------------------------------------------------------------------------------------------------


        public void Setup()
        {
            Window.SetSize(400, 400);
        }

        /// <summary>
        ///     Update runs every frame.
        /// </summary>
        public void Update()
        {
            // Reset the background color to white each frame
            Window.ClearBackground(backgroundColor);

            // Variables
            float mouseX = Input.GetMouseX();
            float mouseY = Input.GetMouseY();
            float headX = headPosition.X + (mouseX - headPosition.X) / 8f; // Higher last variable, higher sensitivity
            float headY = headPosition.Y + (mouseY - headPosition.Y) / 8f;
            float bodyX = bodyPosition.X + (mouseX - bodyPosition.X) / 16f;
            float bodyY = bodyPosition.Y + (mouseY - bodyPosition.Y) / 16f;
            float block1Y = 250 + (mouseY - 250) / 17f;
            float block2Y = 350 + (mouseY - 350) / 19f;

            // -------------------- [ Background ]-----------------------------------------------------------------------------
            Draw.SetLineColor(backgroundBlock1);
            Draw.SetFillColor(backgroundBlock1);
            Draw.Rectangle(0, block1Y, 400, 200);
            Draw.SetLineColor(backgroundBlock2);
            Draw.SetFillColor(backgroundBlock2);
            Draw.Rectangle(0, block2Y, 400, 200);

            // -------------------- [ Body ]-----------------------------------------------------------------------------
            int BODY_WIDTH = 160;
            Draw.SetFillColor(bodyColor);
            Draw.SetLineColor(bodyColor);
            Draw.Rectangle(bodyX - BODY_WIDTH / 2, bodyY, BODY_WIDTH, 130);
            Draw.Triangle( // Left
                bodyX - 80, bodyY + 30, // Upper
                bodyX - 80, bodyY + 105, // Inner 
                bodyX - 110, bodyY + 105 // Outter
                );
            Draw.Triangle( // Right
                bodyX + 80, bodyY + 30, // Upper
                bodyX + 80, bodyY + 105, // Inner 
                bodyX + 110, bodyY + 105 // Outter
                );
            Draw.Triangle( // Left Tri
                bodyX - 80, bodyY + 30, // Upper
                bodyX - 85, bodyY + 60, // Inner 
                bodyX - 100, bodyY + 50 // Outter
                );
            Draw.Triangle( // Right  Tri
               bodyX + 80, bodyY + 30, // Upper
               bodyX + 85, bodyY + 60, // Inner 
               bodyX + 100, bodyY + 50 // Outter
               );


            // -------------------- [ Head ]-----------------------------------------------------------------------------

            // Head Shapes
            Draw.SetFillColor(headColor);
            Draw.SetLineColor(headColor);

            Draw.Ellipse(headX, headY - 35, 180, 56); // Top
            Draw.Ellipse(headX, headY, 237, 116); // Center
            Draw.Ellipse(headX, headY + 31, 232, 92); // Bottoms

            // Tris
            Draw.Triangle( // Left
                headX - 117, headY + 5, // Upper
                headX - 105, headY + 30, // Inner
                headX - 125, headY + 25 // Outer
                );
            Draw.Triangle( // Right
                headX + 117, headY + 5, // Upper
                headX + 105, headY + 30, // Inner
                headX + 125, headY + 25 // Outer
                );
            Draw.Triangle( // Top Left
                headX - 80, headY - 40, // Upper
                headX - 97, headY - 32, // Lower
                headX - 98, headY - 42 // Outer
                );
            Draw.Triangle( // Top Right
               headX + 80, headY - 40, // Upper
               headX + 97, headY - 32, // Lower
               headX + 98, headY - 42 // Outer
               );

            // Mouth
            Vector2 mouthGlobalPosition = new Vector2(headX, headY);

            // Get distance from mouth to mouse position
            float mouseDistance = (mouthGlobalPosition - Input.GetMousePosition()).Length();

            // Reset the game
            if (Input.IsMouseButtonPressed(MouseButton.Left) && cookiedEaten == true && transitionScreenTime >= TRANSITION_SCREEN_TIME)
            {
                cookiedEaten = false;
                transitionScreenTime = 0;
                headPosition = HEAD_HOME_POSITION;
                bodyPosition = BODY_HOME_POSITION;
            }
            else if (Input.IsMouseButtonPressed(MouseButton.Left) && cookiedEaten == false && mouseDistance <= OPEN_DISTANCE)
            { // Eat Cookie
                cookiedEaten = true;
                bodyPosition = new Vector2(2000, 2000);
                headPosition = new Vector2(200, 450);
            }

            // Check if mouse is within range to open mouth
            if (mouseDistance <= OPEN_DISTANCE)
            {
                mouthOpen = true;
            }
            else
            {
                mouthOpen = false;
            }

            // Manage mouth opening and closing
            if (mouthOpen == true && cookiedEaten == false) // Check if mouth can be opened
            {
                // Mouth Open
                Draw.SetFillColor(Color.Black);
                Draw.SetLineColor(Color.Black);
                Draw.Ellipse(headX, headY, 152, 116); // Mouth
            }
            else
            {
                // Mouth Closed
                Draw.SetFillColor(Color.Black);
                Draw.SetLineColor(Color.Black);
                Draw.Ellipse(headX - 31 + OUTLINE_THICKNESS, headY - 27 + OUTLINE_THICKNESS, 122, 37); // Left
                Draw.Ellipse(headX + 31 - OUTLINE_THICKNESS, headY - 27 + OUTLINE_THICKNESS, 122, 37); // Right
                Draw.Rectangle(headX - 50 / 2, headY - 60, 50, 60); // Top Cover 
            }

            // Mouth Cover
            Draw.SetFillColor(headColor);
            Draw.SetLineColor(headColor);
            Draw.Ellipse(headX - 31, headY - 27, 122, 37); // Left
            Draw.Ellipse(headX + 31, headY - 27, 122, 37); // Right
            Draw.Rectangle(headX - 50 / 2, headY - 67, 50, 40); // Top Cover 

            // Eye Variables
            float leftEyeX = headX - 30;
            float rightEyeX = headX + 30;
            float eyeY = headY - 67;

            // Eye OUTLINES
            int EYE_RADIUS = 31;
            Draw.SetFillColor(headColor);
            Draw.Circle(leftEyeX, eyeY, EYE_RADIUS + OUTLINE_THICKNESS); // Left eye outline
            Draw.Circle(rightEyeX, eyeY, EYE_RADIUS + OUTLINE_THICKNESS); // Right eye outline

            // Eyes
            Draw.SetFillColor(Color.White);
            Draw.SetLineColor(Color.White);

            Draw.Circle(leftEyeX, eyeY, EYE_RADIUS); // Left eye
            Draw.Circle(rightEyeX, eyeY, EYE_RADIUS); // Right eye

            // Pupil Varaibles
            float leftPupilX = mouseX - leftEyeX;
            float leftPupilY = mouseY - eyeY;
            float rightPupilX = mouseX - rightEyeX;
            float rightPupilY = mouseY - eyeY;
            Vector2 leftEyeOffset = Input.GetMousePosition() - new Vector2(leftEyeX, eyeY);
            Vector2 rightEyeOffset = Input.GetMousePosition() - new Vector2(rightEyeX, eyeY);
            float leftDistance = leftEyeOffset.Length();
            float rightDistance = rightEyeOffset.Length();

            // Clamp so that the pupils stay within the eyes
            if (leftDistance > EYE_RADIUS - 15)
            {
                leftPupilX = leftEyeX + (leftPupilX / leftDistance) * (EYE_RADIUS - 15);
                leftPupilY = eyeY + (leftPupilY / leftDistance) * (EYE_RADIUS - 15);
            }
            else
            {
                leftPupilX = mouseX;
                leftPupilY = mouseY;
            }
            if (rightDistance > EYE_RADIUS - 15)
            {
                rightPupilX = rightEyeX + (rightPupilX / rightDistance) * (EYE_RADIUS - 15);
                rightPupilY = eyeY + (rightPupilY / rightDistance) * (EYE_RADIUS - 15);
            }
            else
            {
                rightPupilX = mouseX;
                rightPupilY = mouseY;
            }

            // Pupils
            Draw.SetFillColor(Color.Black);
            Draw.Circle(leftPupilX, leftPupilY, 15); // Left pupil
            Draw.Circle(rightPupilX, rightPupilY, 15); // Right pupil

            // Blinking eyes

            // Eyes
            //Console.WriteLine($"Cd: {blinkCd}");
            //Console.WriteLine($"Time: {blinkTime}");
            if (blinkingEnabled)
            { 
                if (blinkTime > 0) // Duration between blinks
                {
                    blinkTime -= Time.DeltaTime;
                }
                else if (blinkCd > 0) // If can blink
                {
                    blinkCd -= Time.DeltaTime;

                    Draw.SetFillColor(headColor);
                    Draw.SetLineColor(headColor);

                    Draw.Circle(leftEyeX, eyeY, EYE_RADIUS);
                    Draw.Circle(rightEyeX, eyeY, EYE_RADIUS);

                    if (blinkCd <= 0) // Set a new blink duration
                    {
                        blinkTime = Random.Float(BLINK_MIN, BLINK_MAX);
                    }
                }
                else // Reset blink cooldown
                {
                    blinkCd = BLINK_TIME;
                }
            }




            // -------------------- [ Cookie ]-----------------------------------------------------------------------------
            if (cookiedEaten == false)
            {
                // Cookie
                Draw.SetFillColor(cookieColor);
                Draw.SetLineColor(cookieColor);
                Draw.Circle(mouseX, mouseY, 25);

                // Chips
                Draw.SetFillColor(chipColor);
                Draw.Circle(mouseX - 4, mouseY + 12, 6);  // BL
                Draw.Circle(mouseX - 12, mouseY - 3, 6); // L
                Draw.Circle(mouseX - 1, mouseY - 13, 7); // T
                Draw.Circle(mouseX + 12, mouseY - 5, 7); // TL
                Draw.Circle(mouseX + 10, mouseY + 10, 6); // BR

            }

            // -------------------- [ Transition Screen ]-----------------------------------------------------------------------------

            if (cookiedEaten == true && transitionScreenTime < TRANSITION_SCREEN_TIME)
            {
                transitionScreenTime += Time.DeltaTime;
                Draw.SetFillColor(Color.Black);
                Draw.SetLineColor(Color.Black);
                Draw.Rectangle(0, 0, Window.Size.X, Window.Size.Y);
            }

            // -------------------- [ Settings ]-----------------------------------------------------------------------------
            // Toggle Blinking
            if (Input.IsKeyboardKeyPressed(KeyboardKey.B))
            {
                blinkingEnabled = blinkingEnabled == true ? false : true;
            }
        }
    }

}
