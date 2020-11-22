using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoverTestApp;

namespace UnitTestRoverApp
{
    [TestClass]
    public class UnitTestRoverApp
    {
        [TestMethod]
        public void Test_Move_Positive_Multiple()
        {
            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testPositiveMove = "Move(L2R3L4R5)";

            string finalOuput = RoverTestApp.Program.Move(testPositiveMove);

            Assert.IsTrue(finalOuput.Contains("Movement done"));
        }

        [TestMethod]
        public void Test_Move_Positive_Multiple_DoubleDigitsSteps()
        {
            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testPositiveMove = "Move(L2R30R25R40)";

            string finalOuput = RoverTestApp.Program.Move(testPositiveMove);

            Assert.IsTrue(finalOuput.Contains("Movement done"));
        }

        [TestMethod]
        public void Test_Move_Negative_Beyond_Plateau_Dimension()
        {

            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testNegativeMove = "Move(L200R200)";

            string finalOuput = RoverTestApp.Program.Move(testNegativeMove);

            Assert.IsTrue(finalOuput.Contains("Invalid"));
        }

        [TestMethod]
        public void Test_Move_Negative_Incorrect_Command()
        {

            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testNegativeMove = "Movement(L2R2)";

            string finalOuput = RoverTestApp.Program.Move(testNegativeMove);

            Assert.IsTrue(finalOuput.Contains("Invalid"));
        }

        [TestMethod]
        public void Test_Move_Negative_Incorrect_Input_Format_Missing_Direction()
        {

            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testNegativeMove = "Movement(L2Y2)";

            string finalOuput = RoverTestApp.Program.Move(testNegativeMove);

            Assert.IsTrue(finalOuput.Contains("Invalid"));
        }

        [TestMethod]
        public void Test_Move_Negative_Incorrect_Input_Format_Missing_StepSize()
        {

            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testNegativeMove = "Movement(LR2)";

            string finalOuput = RoverTestApp.Program.Move(testNegativeMove);

            Assert.IsTrue(finalOuput.Contains("Invalid"));
        }

        [TestMethod]
        public void Test_SetPosition_Positive_Correct_Inputs()
        {
            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testPositivePosition = "SetPosition(1000,1000,N)";

            string finalOuput = RoverTestApp.Program.SetPosition(testPositivePosition);

            Assert.IsTrue(finalOuput.Contains("Initial Position for Rover set"));
        }

        [TestMethod]
        public void Test_SetPosition_Negative_Bad_Command()
        {

            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testNegativePosition = "Position(L200R200)";

            string finalOuput = RoverTestApp.Program.SetPosition(testNegativePosition);

            Assert.IsTrue(finalOuput.Contains("Invalid"));
        }

        [TestMethod]
        public void Test_SetPosition_Negative_No_Inputs()
        {

            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testNegativePosition = "SetPosition()";

            string finalOuput = RoverTestApp.Program.SetPosition(testNegativePosition);

            Assert.IsTrue(finalOuput.Contains("Invalid"));
        }

        [TestMethod]
        public void Test_SetPosition_Negative_Bad_Inputs()
        {

            // Default Rover Values [10,10,N]
            // Setting Default values to Plateau [100 X 100]
            Program.InitializeRoverAndPlateau();

            // Test Data
            string testNegativePosition = "SetPosition(100)";

            string finalOuput = RoverTestApp.Program.SetPosition(testNegativePosition);

            Assert.IsTrue(finalOuput.Contains("Invalid"));
        }
    }
}
