using System.Collections.Generic;
using static BritishCarAuctions.DealerProApp.Api.IntegrationTests.Constants;

namespace Just4Fun.ConsoleApp1.CosmeticConditionConstants
{
    public static class ScreenSpecificKipperviewConstants
    {
        private static Dictionary<string, KipperviewConstants> KipperviewConstantsDictionary { get; } =
            new Dictionary<string, KipperviewConstants>
            {
                [nameof(KipperviewConstants.UWPDesktopScreen)] = KipperviewConstants.UWPDesktopScreen(),
                [nameof(KipperviewConstants.UWPLaptopScreen)] = KipperviewConstants.UWPLaptopScreen(),
                [nameof(KipperviewConstants.IpadRegularScreen)] = KipperviewConstants.IpadRegularScreen(),
                [nameof(KipperviewConstants.IpadMiniScreen)] = KipperviewConstants.IpadMiniScreen()
            };

        public static KipperviewConstants KipperviewConstantsForScreen(string screenType)
        {
            return KipperviewConstantsDictionary[screenType];
        }
    }

    public class KipperviewConstants
    {
        ////Exterior
        public Point ScreenFront { get; private set; }

        public Point Bonnet { get; private set; }

        public Point BumperFront { get; private set; }

        public Point Roof { get; private set; }

        public Point TailgateGlass { get; private set; }

        public Point WheelAndWheelTrimOsr { get; private set; }

        public Point WingOsf { get; private set; }

        public Point DoorOsr { get; private set; }

        public Point QtrPanelNsr { get; private set; }

        public Point WheelAndWheelTrimNsf { get; private set; }

        ////Interior
        public Point DoorPadOsf { get; private set; }

        public Point SeatBackAndBaseCoverOsf { get; private set; }

        public Point QtrPanelTrimOs { get; private set; }

        public Point SwitchesAndControls { get; private set; }

        public Point CarpetsRear { get; private set; }

        public Point TailgatePad { get; private set; }

        public Point DoorPadNsf { get; private set; }

        public Point DoorPadNsr { get; private set; }

        public Point QtrPanelTrimNs { get; private set; }

        public Point RoofLiningAndSunvisor { get; private set; }

        public static KipperviewConstants UWPDesktopScreen()
        {
            return new KipperviewConstants
            {
                ScreenFront = new Point(1110, 600),
                Bonnet = new Point(960, 600),
                BumperFront = new Point(850, 580),
                Roof = new Point(1330, 580),
                TailgateGlass = new Point(1430, 580),
                WheelAndWheelTrimOsr = new Point(1420, 320),
                WingOsf = new Point(1020, 390),
                DoorOsr = new Point(1289, 374),
                QtrPanelNsr = new Point(1480, 770),
                WheelAndWheelTrimNsf = new Point(1045, 845),
                DoorPadOsf = new Point(940, 380),
                SeatBackAndBaseCoverOsf = new Point(1460, 320),
                QtrPanelTrimOs = new Point(1220, 350),
                SwitchesAndControls = new Point(855, 530),
                CarpetsRear = new Point(1220, 555),
                TailgatePad = new Point(1570, 570),
                DoorPadNsf = new Point(920, 835),
                DoorPadNsr = new Point(1090, 815),
                QtrPanelTrimNs = new Point(1210, 810),
                RoofLiningAndSunvisor = new Point(1430, 790)
            };
        }

        public static KipperviewConstants UWPLaptopScreen()
        {
            return new KipperviewConstants
            {
                ScreenFront = new Point(830, 480),
                Bonnet = new Point(700, 500),
                BumperFront = new Point(600, 480),
                Roof = new Point(990, 470),
                TailgateGlass = new Point(1090, 480),
                WheelAndWheelTrimOsr = new Point(1070, 270),
                WingOsf = new Point(732, 315),
                DoorOsr = new Point(1000, 315),
                QtrPanelNsr = new Point(1100, 620),
                WheelAndWheelTrimNsf = new Point(760, 700),
                DoorPadOsf = new Point(680, 300),
                SeatBackAndBaseCoverOsf = new Point(1085, 260),
                QtrPanelTrimOs = new Point(900, 290),
                SwitchesAndControls = new Point(600, 440),
                CarpetsRear = new Point(920, 460),
                TailgatePad = new Point(1200, 470),
                DoorPadNsf = new Point(680, 690),
                DoorPadNsr = new Point(800, 680),
                QtrPanelTrimNs = new Point(900, 672),
                RoofLiningAndSunvisor = new Point(1070, 660)
            };
        }

        public static KipperviewConstants IpadRegularScreen()
        {
            return new KipperviewConstants
            {
                ScreenFront = new Point(584, 407),
                Bonnet = new Point(487, 404),
                BumperFront = new Point(379, 408),
                Roof = new Point(701, 412),
                TailgateGlass = new Point(803, 412),
                WheelAndWheelTrimOsr = new Point(791, 221),
                WingOsf = new Point(497, 264),
                DoorOsr = new Point(717, 264),
                QtrPanelNsr = new Point(812, 524),
                WheelAndWheelTrimNsf = new Point(525, 591),
                DoorPadOsf = new Point(468, 247),
                SeatBackAndBaseCoverOsf = new Point(781, 221),
                QtrPanelTrimOs = new Point(663, 244),
                SwitchesAndControls = new Point(382, 382),
                CarpetsRear = new Point(653, 374),
                TailgatePad = new Point(901, 403),
                DoorPadNsf = new Point(461, 579),
                DoorPadNsr = new Point(557, 575),
                QtrPanelTrimNs = new Point(650, 570),
                RoofLiningAndSunvisor = new Point(801, 550)
            };
        }

        public static KipperviewConstants IpadMiniScreen()
        {
            return new KipperviewConstants
            {
                ScreenFront = new Point(584, 407),
                Bonnet = new Point(487, 404),
                BumperFront = new Point(379, 408),
                Roof = new Point(701, 412),
                TailgateGlass = new Point(803, 412),
                WheelAndWheelTrimOsr = new Point(791, 221),
                WingOsf = new Point(497, 264),
                DoorOsr = new Point(717, 264),
                QtrPanelNsr = new Point(812, 524),
                WheelAndWheelTrimNsf = new Point(525, 591),
                DoorPadOsf = new Point(468, 247),
                SeatBackAndBaseCoverOsf = new Point(781, 221),
                QtrPanelTrimOs = new Point(663, 244),
                SwitchesAndControls = new Point(382, 382),
                CarpetsRear = new Point(653, 374),
                TailgatePad = new Point(901, 403),
                DoorPadNsf = new Point(461, 579),
                DoorPadNsr = new Point(557, 575),
                QtrPanelTrimNs = new Point(650, 570),
                RoofLiningAndSunvisor = new Point(801, 550)
            };
        }
    }
}
