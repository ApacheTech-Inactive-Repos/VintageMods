using System;
using System.Collections.Generic;
using Cairo;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace VintageMods.Mods.WaypointExtensions.Gui.Icons
{
    public class IconUtil
    {
        private readonly ICoreClientAPI _capi;

        /// <summary>
        ///     Initialises a new instance of the <see cref="IconUtil"/> class.
        /// </summary>
        /// <param name="capi">The Client API.</param>
        public IconUtil(ICoreClientAPI capi)
        {
            _capi = capi;
        }

        /// <summary>
        ///     Generates the texture.  
        /// </summary>
        /// <param name="width">The width of the drawing</param>
        /// <param name="height">The height of the drawing.</param>
        /// <param name="drawHandler">A delegate which handles the drawing.</param>
        /// <returns>The resulting built texture.</returns>
        public LoadedTexture GenTexture(int width, int height, DrawDelegate drawHandler)
        {
            var surface = new ImageSurface(Format.Argb32, width, height);
            var ctx = new Context(surface);

            drawHandler(ctx, surface);

            var textureId = _capi.Gui.LoadCairoTexture(surface, true);

            surface.Dispose();
            ctx.Dispose();

            return new LoadedTexture(_capi)
            {
                TextureId = textureId,
                Width = width,
                Height = height
            };
        }

        /// <summary>
        ///     Draws the icon.
        /// </summary>
        /// <param name="cr">The context.</param>
        /// <param name="type">The type to draw</param>
        /// <param name="x">X position of the Icon.</param>
        /// <param name="y">Y position of the Icon.</param>
        /// <param name="width">Width of the Icon.</param>
        /// <param name="height">Height of the Icon.</param>
        /// <param name="rgba">The color of the icon.</param>
        public void DrawIcon(Context cr, string type, double x, double y, double width, double height, double[] rgba)
        {
            DrawIconInt(cr, type, (int)x, (int)y, (float)width, (float)height, rgba);
        }

        /// <summary>
        ///     Draws the icon.
        /// </summary>
        /// <param name="cr">The context.</param>
        /// <param name="type">The type of icon to draw</param>
        /// <param name="x">X position of the Icon.</param>
        /// <param name="y">Y position of the Icon.</param>
        /// <param name="width">Width of the Icon.</param>
        /// <param name="height">Height of the Icon.</param>
        /// <param name="rgba">The color of the icon.</param>
        private static void DrawIconInt(Context cr, string type, int x, int y, float width, float height, double[] rgba)
        {
            switch (type)
            {
                case "wpTree":
                    DrawWaypointTree(cr, x, y, width, height, rgba);
                    break;

                case "wpShovel1":
                    DrawWaypointShovel1(cr, x, y, width, height, rgba);
                    break;

                case "wpShovel2":
                    DrawWaypointShovel2(cr, x, y, width, height, rgba);
                    break;

                case "wpGem1":
                    DrawWaypointGem1(cr, x, y, width, height, rgba);
                    break;

                case "wpGem2":
                    DrawWaypointGem2(cr, x, y, width, height, rgba);
                    break;

                case "wpMushroom1":
                    DrawWaypointMushroom1(cr, x, y, width, height, rgba);
                    break;

                case "wpMushroom2":
                    DrawWaypointMushroom2(cr, x, y, width, height, rgba);
                    break;

                case "wpWater":
                    DrawWaypointWater(cr, x, y, width, height, rgba);
                    break;

                case "wpMeteor":
                    DrawWaypointMeteor(cr, x, y, width, height, rgba);
                    break;

                case "wpFlower":
                    DrawWaypointFlower(cr, x, y, width, height, rgba);
                    break;

            }
        }

        private static void DrawWaypointTree(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;
            cr.Save();
            float w = 590;
            float h = 590;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(537.804688, 174.6875);
            cr.CurveTo(537.804688, 129.914063, 503.828125, 93.089844, 460.253906, 88.566406);
            cr.CurveTo(448.023438, 55.585938, 416.371094, 32.035156, 379.125, 32.035156);
            cr.CurveTo(362.820313, 32.035156, 347.625, 36.625, 334.609375, 44.457031);
            cr.CurveTo(319.808594, 17.949219, 291.511719, 0, 258.992188, 0);
            cr.CurveTo(215.875, 0, 180.214844, 31.554688, 173.597656, 72.808594);
            cr.CurveTo(170.078125, 72.378906, 166.523438, 72.082031, 162.886719, 72.082031);
            cr.CurveTo(115.066406, 72.082031, 76.289063, 110.847656, 76.289063, 158.679688);
            cr.CurveTo(76.289063, 161.023438, 76.460938, 163.316406, 76.644531, 165.613281);
            cr.CurveTo(52.394531, 180.960938, 36.253906, 207.945313, 36.253906, 238.765625);
            cr.CurveTo(36.253906, 266.011719, 48.855469, 290.277344, 68.523438, 306.152344);
            cr.CurveTo(68.4375, 307.710938, 68.285156, 309.261719, 68.285156, 310.839844);
            cr.CurveTo(68.285156, 358.660156, 107.054688, 397.4375, 154.882813, 397.4375);
            cr.CurveTo(169.21875, 397.4375, 182.699219, 393.898438, 194.605469, 387.742188);
            cr.CurveTo(211.101563, 399.589844, 234.722656, 414.410156, 246.15625, 411.457031);
            cr.CurveTo(246.15625, 411.457031, 250.414063, 477.359375, 249.496094, 494.097656);
            cr.CurveTo(247.746094, 525.9375, 238.191406, 561.386719, 231.46875, 590.074219);
            cr.LineTo(335.585938, 590.074219);
            cr.CurveTo(335.585938, 590.074219, 320.238281, 526.121094, 319.570313, 504.769531);
            cr.CurveTo(318.898438, 483.414063, 326.242188, 444.09375, 326.242188, 444.09375);
            cr.LineTo(362.363281, 406.734375);
            cr.CurveTo(376.265625, 416.238281, 393.058594, 421.640625, 411.167969, 421.640625);
            cr.CurveTo(455.941406, 421.640625, 492.765625, 387.578125, 497.289063, 344.003906);
            cr.CurveTo(530.269531, 331.773438, 553.820313, 300.035156, 553.820313, 262.789063);
            cr.CurveTo(553.820313, 240.792969, 545.558594, 220.789063, 532.058594, 205.507813);
            cr.CurveTo(535.710938, 195.925781, 537.804688, 185.5625, 537.804688, 174.6875);
            cr.ClosePath();
            cr.MoveTo(214.609375, 373.445313);
            cr.CurveTo(221.554688, 366.816406, 227.378906, 359.070313, 231.824219, 350.476563);
            cr.LineTo(248.824219, 386.09375);
            cr.CurveTo(248.816406, 386.097656, 239.570313, 390.179688, 214.609375, 373.445313);
            cr.ClosePath();
            cr.MoveTo(278.183594, 395.4375);
            cr.CurveTo(269.386719, 397.035156, 254.402344, 369.945313, 243.765625, 347.921875);
            cr.CurveTo(255.558594, 353.9375, 268.867188, 357.398438, 283.019531, 357.398438);
            cr.CurveTo(286.65625, 357.398438, 290.222656, 357.101563, 293.742188, 356.660156);
            cr.CurveTo(291.007813, 374.285156, 286.1875, 393.976563, 278.183594, 395.4375);
            cr.ClosePath();
            cr.MoveTo(315.5625, 412.773438);
            cr.CurveTo(295.214844, 418.425781, 307.394531, 376.273438, 313.230469, 351.871094);
            cr.CurveTo(317.445313, 350.304688, 321.53125, 348.457031, 325.410156, 346.265625);
            cr.CurveTo(327.753906, 364.054688, 335.480469, 380.097656, 346.929688, 392.789063);
            cr.CurveTo(337.011719, 401.597656, 325.59375, 409.992188, 315.5625, 412.773438);
            cr.ClosePath();
            cr.MoveTo(315.5625, 412.773438);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }

        private static void DrawWaypointWater(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;

            cr.Save();
            float w = 295;
            float h = 295;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(158.304688, 6.925781);
            cr.CurveTo(156.363281, 2.707031, 152.144531, 0, 147.503906, 0);
            cr.CurveTo(142.859375, 0, 138.636719, 2.699219, 136.695313, 6.917969);
            cr.CurveTo(106.785156, 71.972656, 53.738281, 155.703125, 53.738281, 201.242188);
            cr.CurveTo(53.738281, 253.023438, 95.71875, 295, 147.5, 295);
            cr.CurveTo(199.28125, 295, 241.261719, 253.023438, 241.261719, 201.242188);
            cr.CurveTo(241.261719, 155.703125, 188.214844, 71.976563, 158.304688, 6.925781);
            cr.ClosePath();
            cr.MoveTo(143.496094, 265.132813);
            cr.CurveTo(108.851563, 265.132813, 80.671875, 236.949219, 80.671875, 202.308594);
            cr.CurveTo(80.671875, 195.394531, 86.273438, 189.792969, 93.183594, 189.792969);
            cr.CurveTo(100.097656, 189.792969, 105.699219, 195.394531, 105.699219, 202.308594);
            cr.CurveTo(105.699219, 223.148438, 122.65625, 240.101563, 143.496094, 240.101563);
            cr.CurveTo(150.40625, 240.101563, 156.007813, 245.707031, 156.007813, 252.617188);
            cr.CurveTo(156.007813, 259.527344, 150.40625, 265.132813, 143.496094, 265.132813);
            cr.ClosePath();
            cr.MoveTo(143.496094, 265.132813);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }

        private static void DrawWaypointShovel1(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;

            cr.Save();
            float w = 215;
            float h = 215;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(212.800781, 32.980469);
            cr.LineTo(182.019531, 2.199219);
            cr.CurveTo(180.609375, 0.789063, 178.703125, 0, 176.710938, 0);
            cr.CurveTo(174.71875, 0, 172.808594, 0.789063, 171.402344, 2.199219);
            cr.CurveTo(161.15625, 12.445313, 157.082031, 26.113281, 159.835938, 37.230469);
            cr.LineTo(74.125, 122.941406);
            cr.LineTo(58.574219, 107.386719);
            cr.CurveTo(57.164063, 105.980469, 55.253906, 105.191406, 53.265625, 105.191406);
            cr.CurveTo(51.273438, 105.191406, 49.363281, 105.980469, 47.957031, 107.386719);
            cr.LineTo(12.355469, 142.992188);
            cr.CurveTo(4.386719, 150.957031, 0, 161.550781, 0, 172.816406);
            cr.CurveTo(0, 184.085938, 4.386719, 194.679688, 12.355469, 202.644531);
            cr.CurveTo(20.320313, 210.613281, 30.914063, 215, 42.183594, 215);
            cr.CurveTo(53.449219, 215, 64.042969, 210.613281, 72.007813, 202.644531);
            cr.LineTo(107.613281, 167.042969);
            cr.CurveTo(109.019531, 165.636719, 109.8125, 163.726563, 109.8125, 161.734375);
            cr.CurveTo(109.8125, 159.742188, 109.019531, 157.835938, 107.613281, 156.425781);
            cr.LineTo(92.058594, 140.875);
            cr.LineTo(177.742188, 55.191406);
            cr.CurveTo(180.011719, 55.769531, 182.398438, 56.078125, 184.867188, 56.078125);
            cr.CurveTo(194.6875, 56.078125, 204.871094, 51.527344, 212.800781, 43.597656);
            cr.CurveTo(214.210938, 42.191406, 215, 40.28125, 215, 38.289063);
            cr.CurveTo(215, 36.296875, 214.210938, 34.390625, 212.800781, 32.980469);
            cr.ClosePath();
            cr.MoveTo(184.867188, 41.066406);
            cr.CurveTo(182.554688, 41.066406, 179.285156, 40.554688, 176.867188, 38.132813);
            cr.CurveTo(172.628906, 33.898438, 173.09375, 25.972656, 177.367188, 18.777344);
            cr.LineTo(196.222656, 37.636719);
            cr.CurveTo(192.519531, 39.835938, 188.554688, 41.066406, 184.867188, 41.066406);
            cr.ClosePath();
            cr.MoveTo(184.867188, 41.066406);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }

        private static void DrawWaypointShovel2(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;

            cr.Save();
            float w = 456;
            float h = 456;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(244.578125, 290.367188);
            cr.CurveTo(243.28125, 267.820313, 238.789063, 245.980469, 231.792969, 229.640625);
            cr.CurveTo(228.535156, 222.027344, 219.71875, 218.5, 212.105469, 221.757813);
            cr.LineTo(182.605469, 234.386719);
            cr.LineTo(169.160156, 202.980469);
            cr.CurveTo(165.898438, 195.367188, 157.085938, 191.839844, 149.472656, 195.097656);
            cr.LineTo(146.488281, 196.375);
            cr.LineTo(93.214844, 71.941406);
            cr.CurveTo(100.171875, 66.253906, 105.625, 58.878906, 109.058594, 50.304688);
            cr.CurveTo(114.40625, 36.953125, 114.234375, 22.316406, 108.570313, 9.09375);
            cr.CurveTo(107.007813, 5.4375, 104.050781, 2.550781, 100.359375, 1.074219);
            cr.CurveTo(96.667969, -0.40625, 92.539063, -0.355469, 88.882813, 1.210938);
            cr.LineTo(17.445313, 31.796875);
            cr.CurveTo(9.832031, 35.054688, 6.304688, 43.871094, 9.5625, 51.484375);
            cr.CurveTo(15.226563, 64.707031, 25.695313, 74.933594, 39.050781, 80.28125);
            cr.CurveTo(45.527344, 82.875, 52.304688, 84.167969, 59.078125, 84.167969);
            cr.CurveTo(61.269531, 84.167969, 63.460938, 84.015625, 65.644531, 83.746094);
            cr.LineTo(118.921875, 208.183594);
            cr.LineTo(115.9375, 209.460938);
            cr.CurveTo(112.285156, 211.023438, 109.398438, 213.976563, 107.921875, 217.671875);
            cr.CurveTo(106.441406, 221.363281, 106.492188, 225.492188, 108.058594, 229.148438);
            cr.LineTo(121.503906, 260.554688);
            cr.LineTo(92.003906, 273.183594);
            cr.CurveTo(88.347656, 274.746094, 85.460938, 277.703125, 83.984375, 281.394531);
            cr.CurveTo(82.507813, 285.085938, 82.554688, 289.214844, 84.121094, 292.871094);
            cr.CurveTo(93.910156, 315.734375, 114.875, 342.492188, 138.835938, 362.695313);
            cr.CurveTo(139.804688, 363.511719, 140.828125, 364.355469, 141.890625, 365.214844);
            cr.CurveTo(157.390625, 347.644531, 178.988281, 335.570313, 203.335938, 332.347656);
            cr.CurveTo(213.308594, 314.847656, 227.609375, 300.46875, 244.578125, 290.367188);
            cr.ClosePath();
            cr.MoveTo(50.199219, 52.4375);
            cr.CurveTo(49.410156, 52.125, 48.644531, 51.769531, 47.90625, 51.378906);
            cr.LineTo(82.035156, 36.769531);
            cr.CurveTo(81.804688, 37.574219, 81.535156, 38.371094, 81.21875, 39.15625);
            cr.CurveTo(78.851563, 45.074219, 74.320313, 49.714844, 68.460938, 52.222656);
            cr.CurveTo(62.601563, 54.730469, 56.113281, 54.808594, 50.199219, 52.4375);
            cr.ClosePath();
            cr.MoveTo(50.199219, 52.4375);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(447.152344, 439.238281);
            cr.CurveTo(439.800781, 413.292969, 416.105469, 395.039063, 389.238281, 394.417969);
            cr.CurveTo(389.324219, 392.84375, 389.367188, 391.265625, 389.367188, 389.6875);
            cr.CurveTo(389.367188, 342.480469, 350.964844, 304.078125, 303.757813, 304.078125);
            cr.CurveTo(266.691406, 304.078125, 234.503906, 327.667969, 222.816406, 361.765625);
            cr.CurveTo(220.703125, 361.570313, 218.578125, 361.472656, 216.453125, 361.472656);
            cr.CurveTo(178.171875, 361.472656, 147.027344, 392.617188, 147.027344, 430.898438);
            cr.CurveTo(147.027344, 435.851563, 147.558594, 440.804688, 148.601563, 445.621094);
            cr.CurveTo(149.914063, 451.679688, 155.277344, 456, 161.472656, 456);
            cr.LineTo(434.480469, 456);
            cr.CurveTo(438.605469, 456, 442.496094, 454.066406, 444.984375, 450.773438);
            cr.CurveTo(447.476563, 447.476563, 448.277344, 443.207031, 447.152344, 439.238281);
            cr.ClosePath();
            cr.MoveTo(447.152344, 439.238281);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }

        private static void DrawWaypointGem1(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;

            cr.Save();
            float w = 232;
            float h = 232;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(110.382813, 15.335938);
            cr.LineTo(65.089844, 80.109375);
            cr.LineTo(40.105469, 15.664063);
            cr.ClosePath();
            cr.MoveTo(110.382813, 15.335938);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(35.257813, 19.695313);
            cr.LineTo(59.828125, 83.070313);
            cr.LineTo(0, 82.707031);
            cr.ClosePath();
            cr.MoveTo(35.257813, 19.695313);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(1.300781, 88.683594);
            cr.LineTo(62.097656, 89.058594);
            cr.LineTo(107.226563, 216.664063);
            cr.ClosePath();
            cr.MoveTo(1.300781, 88.683594);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(112.699219, 214.234375);
            cr.LineTo(68.445313, 89.09375);
            cr.LineTo(112.972656, 89.367188);
            cr.ClosePath();
            cr.MoveTo(112.699219, 214.234375);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(112.992188, 83.398438);
            cr.LineTo(70.253906, 83.136719);
            cr.LineTo(112.902344, 22.148438);
            cr.LineTo(113.128906, 21.832031);
            cr.LineTo(113.015625, 71.859375);
            cr.ClosePath();
            cr.MoveTo(112.992188, 83.398438);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(232, 82.707031);
            cr.LineTo(172.171875, 83.070313);
            cr.LineTo(196.738281, 19.695313);
            cr.ClosePath();
            cr.MoveTo(232, 82.707031);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(191.894531, 15.664063);
            cr.LineTo(166.910156, 80.109375);
            cr.LineTo(121.617188, 15.335938);
            cr.ClosePath();
            cr.MoveTo(191.894531, 15.664063);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(118.984375, 71.859375);
            cr.LineTo(118.871094, 21.832031);
            cr.LineTo(119.09375, 22.148438);
            cr.LineTo(161.746094, 83.136719);
            cr.LineTo(119.007813, 83.398438);
            cr.ClosePath();
            cr.MoveTo(118.984375, 71.859375);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(119.019531, 89.367188);
            cr.LineTo(163.554688, 89.09375);
            cr.LineTo(119.300781, 214.234375);
            cr.ClosePath();
            cr.MoveTo(119.019531, 89.367188);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(124.777344, 216.664063);
            cr.LineTo(169.902344, 89.058594);
            cr.LineTo(230.699219, 88.683594);
            cr.ClosePath();
            cr.MoveTo(124.777344, 216.664063);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }

        private static void DrawWaypointGem2(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;

            cr.Save();
            float w = 32;
            float h = 32;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(9.53125, 6);
            cr.LineTo(4.21875, 12.375);
            cr.LineTo(3.71875, 12.96875);
            cr.LineTo(4.21875, 13.625);
            cr.LineTo(15.21875, 27.625);
            cr.LineTo(16, 28.625);
            cr.LineTo(16.78125, 27.625);
            cr.LineTo(27.78125, 13.625);
            cr.LineTo(28.28125, 12.96875);
            cr.LineTo(27.78125, 12.375);
            cr.LineTo(22.46875, 6);
            cr.ClosePath();
            cr.MoveTo(10.46875, 8);
            cr.LineTo(14.125, 8);
            cr.LineTo(11.4375, 12);
            cr.LineTo(7.125, 12);
            cr.ClosePath();
            cr.MoveTo(17.875, 8);
            cr.LineTo(21.53125, 8);
            cr.LineTo(24.875, 12);
            cr.LineTo(20.5625, 12);
            cr.ClosePath();
            cr.MoveTo(16, 8.84375);
            cr.LineTo(18.125, 12);
            cr.LineTo(13.875, 12);
            cr.ClosePath();
            cr.MoveTo(7.03125, 14);
            cr.LineTo(11.25, 14);
            cr.LineTo(13.625, 22.40625);
            cr.ClosePath();
            cr.MoveTo(13.3125, 14);
            cr.LineTo(18.65625, 14);
            cr.LineTo(16, 23.3125);
            cr.ClosePath();
            cr.MoveTo(20.75, 14);
            cr.LineTo(24.96875, 14);
            cr.LineTo(18.375, 22.375);
            cr.ClosePath();
            cr.MoveTo(20.75, 14);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }

        private static void DrawWaypointFlower(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;

            cr.Save();
            float w = 465;
            float h = 465;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(391.355469, 138.054688);
            cr.CurveTo(385.027344, 135.875, 378.023438, 138.148438, 374.183594, 143.628906);
            cr.LineTo(247.496094, 324.472656);
            cr.LineTo(247.496094, 233.582031);
            cr.CurveTo(296.976563, 226.304688, 335.082031, 183.570313, 335.082031, 132.101563);
            cr.LineTo(335.082031, 14.996094);
            cr.CurveTo(335.082031, 9.199219, 331.738281, 3.921875, 326.496094, 1.441406);
            cr.CurveTo(321.257813, -1.039063, 315.058594, -0.277344, 310.570313, 3.402344);
            cr.LineTo(264.21875, 41.429688);
            cr.LineTo(244.222656, 16.371094);
            cr.CurveTo(241.375, 12.808594, 237.0625, 10.730469, 232.5, 10.730469);
            cr.CurveTo(227.9375, 10.730469, 223.625, 12.804688, 220.777344, 16.371094);
            cr.LineTo(200.78125, 41.429688);
            cr.LineTo(154.425781, 3.402344);
            cr.CurveTo(149.945313, -0.273438, 143.742188, -1.039063, 138.503906, 1.441406);
            cr.CurveTo(133.261719, 3.917969, 129.917969, 9.199219, 129.917969, 14.996094);
            cr.LineTo(129.917969, 132.097656);
            cr.CurveTo(129.917969, 183.570313, 168.023438, 226.304688, 217.503906, 233.582031);
            cr.LineTo(217.503906, 324.472656);
            cr.LineTo(90.8125, 143.628906);
            cr.CurveTo(86.976563, 138.148438, 79.972656, 135.871094, 73.644531, 138.054688);
            cr.CurveTo(67.316406, 140.234375, 63.203125, 146.34375, 63.554688, 153.023438);
            cr.CurveTo(63.671875, 155.234375, 66.660156, 207.828125, 87.734375, 268.601563);
            cr.CurveTo(100.222656, 304.617188, 116.519531, 336.035156, 136.171875, 361.984375);
            cr.CurveTo(158.949219, 392.0625, 186.28125, 414.78125, 217.503906, 429.6875);
            cr.LineTo(217.503906, 450.003906);
            cr.CurveTo(217.503906, 458.285156, 224.21875, 465, 232.5, 465);
            cr.CurveTo(240.78125, 465, 247.496094, 458.285156, 247.496094, 450.003906);
            cr.LineTo(247.496094, 429.6875);
            cr.CurveTo(278.71875, 414.78125, 306.050781, 392.0625, 328.828125, 361.984375);
            cr.CurveTo(348.480469, 336.035156, 364.777344, 304.617188, 377.265625, 268.601563);
            cr.CurveTo(398.339844, 207.828125, 401.328125, 155.234375, 401.445313, 153.023438);
            cr.CurveTo(401.796875, 146.34375, 397.679688, 140.234375, 391.355469, 138.054688);
            cr.ClosePath();
            cr.MoveTo(391.355469, 138.054688);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }

        private static void DrawWaypointMeteor(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;

            cr.Save();
            float w = 504;
            float h = 504;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(168.3125, 347.730469);
            cr.CurveTo(165.21875, 350.820313, 165.21875, 355.847656, 168.3125, 358.9375);
            cr.CurveTo(169.855469, 360.484375, 171.871094, 361.273438, 173.894531, 361.269531);
            cr.CurveTo(175.953125, 361.269531, 177.972656, 360.480469, 179.515625, 358.9375);
            cr.CurveTo(182.605469, 355.847656, 182.605469, 350.816406, 179.515625, 347.730469);
            cr.CurveTo(176.421875, 344.644531, 171.394531, 344.644531, 168.3125, 347.730469);
            cr.ClosePath();
            cr.MoveTo(168.3125, 347.730469);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(123.527344, 302.945313);
            cr.CurveTo(120.394531, 306.039063, 120.394531, 311.066406, 123.527344, 314.15625);
            cr.CurveTo(125.066406, 315.699219, 127.085938, 316.449219, 129.101563, 316.449219);
            cr.CurveTo(131.125, 316.449219, 133.148438, 315.699219, 134.6875, 314.15625);
            cr.CurveTo(137.777344, 311.0625, 137.777344, 306.035156, 134.6875, 302.945313);
            cr.CurveTo(131.605469, 299.859375, 126.609375, 299.859375, 123.527344, 302.945313);
            cr.ClosePath();
            cr.MoveTo(123.527344, 302.945313);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(190.722656, 370.144531);
            cr.CurveTo(187.636719, 373.238281, 187.636719, 378.261719, 190.722656, 381.351563);
            cr.CurveTo(192.269531, 382.894531, 194.285156, 383.644531, 196.308594, 383.644531);
            cr.CurveTo(198.328125, 383.644531, 200.382813, 382.894531, 201.929688, 381.351563);
            cr.CurveTo(205.019531, 378.261719, 205.019531, 373.238281, 201.929688, 370.144531);
            cr.CurveTo(198.804688, 367.058594, 193.8125, 367.058594, 190.722656, 370.144531);
            cr.ClosePath();
            cr.MoveTo(190.722656, 370.144531);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(145.898438, 325.316406);
            cr.CurveTo(142.8125, 328.449219, 142.8125, 333.4375, 145.898438, 336.523438);
            cr.CurveTo(147.4375, 338.074219, 149.5, 338.859375, 151.523438, 338.859375);
            cr.CurveTo(153.539063, 338.859375, 155.5625, 338.074219, 157.105469, 336.523438);
            cr.CurveTo(160.191406, 333.433594, 160.191406, 328.449219, 157.105469, 325.316406);
            cr.CurveTo(154.015625, 322.234375, 148.984375, 322.234375, 145.898438, 325.316406);
            cr.ClosePath();
            cr.MoveTo(145.898438, 325.316406);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(269.085938, 437.34375);
            cr.CurveTo(266, 434.257813, 261.003906, 434.257813, 257.921875, 437.34375);
            cr.CurveTo(254.832031, 440.433594, 254.832031, 445.464844, 257.921875, 448.550781);
            cr.CurveTo(259.46875, 450.09375, 261.484375, 450.84375, 263.503906, 450.847656);
            cr.CurveTo(265.519531, 450.847656, 267.535156, 450.09375, 269.085938, 448.550781);
            cr.CurveTo(272.210938, 445.464844, 272.210938, 440.433594, 269.085938, 437.34375);
            cr.ClosePath();
            cr.MoveTo(269.085938, 437.34375);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(235.507813, 414.925781);
            cr.CurveTo(232.425781, 418.019531, 232.425781, 423.050781, 235.507813, 426.136719);
            cr.CurveTo(237.050781, 427.679688, 239.078125, 428.472656, 241.09375, 428.472656);
            cr.CurveTo(243.148438, 428.472656, 245.171875, 427.679688, 246.710938, 426.136719);
            cr.CurveTo(249.800781, 423.046875, 249.800781, 418.015625, 246.710938, 414.925781);
            cr.CurveTo(243.625, 411.839844, 238.59375, 411.839844, 235.507813, 414.925781);
            cr.ClosePath();
            cr.MoveTo(235.507813, 414.925781);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(213.09375, 392.558594);
            cr.CurveTo(210.007813, 395.648438, 210.007813, 400.636719, 213.09375, 403.726563);
            cr.CurveTo(214.640625, 405.265625, 216.699219, 406.0625, 218.714844, 406.0625);
            cr.CurveTo(220.734375, 406.0625, 222.757813, 405.269531, 224.300781, 403.726563);
            cr.CurveTo(227.386719, 400.636719, 227.386719, 395.648438, 224.300781, 392.558594);
            cr.CurveTo(221.210938, 389.472656, 216.179688, 389.472656, 213.09375, 392.558594);
            cr.ClosePath();
            cr.MoveTo(213.09375, 392.558594);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(325.398438, 162.53125);
            cr.CurveTo(322.308594, 165.625, 322.308594, 170.652344, 325.398438, 173.742188);
            cr.CurveTo(326.941406, 175.285156, 328.960938, 176.070313, 330.984375, 176.070313);
            cr.CurveTo(333.035156, 176.070313, 335.058594, 175.289063, 336.605469, 173.742188);
            cr.CurveTo(339.691406, 170.648438, 339.691406, 165.621094, 336.605469, 162.53125);
            cr.CurveTo(333.511719, 159.445313, 328.484375, 159.445313, 325.398438, 162.53125);
            cr.ClosePath();
            cr.MoveTo(325.398438, 162.53125);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(302.980469, 140.160156);
            cr.CurveTo(299.898438, 143.246094, 299.898438, 148.234375, 302.980469, 151.324219);
            cr.CurveTo(304.523438, 152.90625, 306.546875, 153.660156, 308.605469, 153.660156);
            cr.CurveTo(310.625, 153.660156, 312.644531, 152.90625, 314.195313, 151.324219);
            cr.CurveTo(317.277344, 148.234375, 317.277344, 143.246094, 314.195313, 140.160156);
            cr.CurveTo(311.101563, 137.03125, 306.070313, 137.03125, 302.980469, 140.160156);
            cr.ClosePath();
            cr.MoveTo(302.980469, 140.160156);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(280.609375, 117.742188);
            cr.CurveTo(277.476563, 120.835938, 277.476563, 125.863281, 280.609375, 128.949219);
            cr.CurveTo(282.15625, 130.496094, 284.175781, 131.246094, 286.191406, 131.246094);
            cr.CurveTo(288.210938, 131.246094, 290.230469, 130.496094, 291.773438, 128.949219);
            cr.CurveTo(294.90625, 125.859375, 294.90625, 120.832031, 291.773438, 117.742188);
            cr.CurveTo(288.691406, 114.660156, 283.695313, 114.660156, 280.609375, 117.742188);
            cr.ClosePath();
            cr.MoveTo(280.609375, 117.742188);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(258.195313, 95.332031);
            cr.CurveTo(255.109375, 98.425781, 255.109375, 103.453125, 258.195313, 106.542969);
            cr.CurveTo(259.742188, 108.085938, 261.757813, 108.875, 263.785156, 108.875);
            cr.CurveTo(265.800781, 108.875, 267.859375, 108.085938, 269.410156, 106.542969);
            cr.CurveTo(272.492188, 103.449219, 272.492188, 98.421875, 269.410156, 95.332031);
            cr.CurveTo(266.316406, 92.246094, 261.285156, 92.246094, 258.195313, 95.332031);
            cr.ClosePath();
            cr.MoveTo(258.195313, 95.332031);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(403.796875, 229.722656);
            cr.CurveTo(400.710938, 226.640625, 395.679688, 226.640625, 392.59375, 229.722656);
            cr.CurveTo(389.507813, 232.820313, 389.507813, 237.84375, 392.59375, 240.929688);
            cr.CurveTo(394.132813, 242.480469, 396.160156, 243.265625, 398.175781, 243.265625);
            cr.CurveTo(400.234375, 243.265625, 402.253906, 242.480469, 403.796875, 240.929688);
            cr.CurveTo(406.886719, 237.84375, 406.886719, 232.816406, 403.796875, 229.722656);
            cr.ClosePath();
            cr.MoveTo(403.796875, 229.722656);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(370.183594, 207.355469);
            cr.CurveTo(367.089844, 210.449219, 367.089844, 215.4375, 370.183594, 218.5625);
            cr.CurveTo(371.730469, 220.109375, 373.785156, 220.859375, 375.804688, 220.859375);
            cr.CurveTo(377.820313, 220.859375, 379.839844, 220.113281, 381.386719, 218.5625);
            cr.CurveTo(384.476563, 215.4375, 384.476563, 210.445313, 381.386719, 207.355469);
            cr.CurveTo(378.300781, 204.273438, 373.269531, 204.273438, 370.183594, 207.355469);
            cr.ClosePath();
            cr.MoveTo(370.183594, 207.355469);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(347.804688, 184.945313);
            cr.CurveTo(344.722656, 188.035156, 344.722656, 193.058594, 347.804688, 196.152344);
            cr.CurveTo(349.351563, 197.695313, 351.371094, 198.488281, 353.394531, 198.488281);
            cr.CurveTo(355.414063, 198.488281, 357.433594, 197.695313, 359.019531, 196.152344);
            cr.CurveTo(362.066406, 193.058594, 362.066406, 188.035156, 359.019531, 184.945313);
            cr.CurveTo(355.886719, 181.855469, 350.894531, 181.855469, 347.804688, 184.945313);
            cr.ClosePath();
            cr.MoveTo(347.804688, 184.945313);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(91.882813, 100.277344);
            cr.CurveTo(88.796875, 103.375, 88.796875, 108.402344, 91.882813, 111.492188);
            cr.CurveTo(93.425781, 113.035156, 95.492188, 113.78125, 97.511719, 113.78125);
            cr.CurveTo(99.527344, 113.78125, 101.546875, 113.035156, 103.09375, 111.492188);
            cr.CurveTo(106.179688, 108.398438, 106.179688, 103.371094, 103.09375, 100.277344);
            cr.CurveTo(100.003906, 97.195313, 95.011719, 97.195313, 91.882813, 100.277344);
            cr.ClosePath();
            cr.MoveTo(91.882813, 100.277344);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(114.296875, 122.695313);
            cr.CurveTo(111.210938, 125.785156, 111.210938, 130.773438, 114.296875, 133.90625);
            cr.CurveTo(115.84375, 135.441406, 117.863281, 136.199219, 119.921875, 136.199219);
            cr.CurveTo(121.9375, 136.199219, 123.957031, 135.445313, 125.507813, 133.90625);
            cr.CurveTo(128.589844, 130.773438, 128.589844, 125.785156, 125.507813, 122.695313);
            cr.CurveTo(122.410156, 119.609375, 117.382813, 119.609375, 114.296875, 122.695313);
            cr.ClosePath();
            cr.MoveTo(114.296875, 122.695313);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(24.6875, 33.082031);
            cr.CurveTo(21.601563, 36.167969, 21.601563, 41.203125, 24.6875, 44.289063);
            cr.CurveTo(26.269531, 45.832031, 28.289063, 46.585938, 30.308594, 46.585938);
            cr.CurveTo(32.324219, 46.585938, 34.347656, 45.832031, 35.894531, 44.289063);
            cr.CurveTo(38.980469, 41.199219, 38.980469, 36.167969, 35.894531, 33.082031);
            cr.CurveTo(32.800781, 29.992188, 27.8125, 29.992188, 24.6875, 33.082031);
            cr.ClosePath();
            cr.MoveTo(24.6875, 33.082031);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(2.3125, 10.667969);
            cr.CurveTo(-0.769531, 13.761719, -0.769531, 18.789063, 2.3125, 21.875);
            cr.CurveTo(3.855469, 23.421875, 5.878906, 24.210938, 7.898438, 24.207031);
            cr.CurveTo(9.917969, 24.207031, 11.976563, 23.421875, 13.515625, 21.875);
            cr.CurveTo(16.605469, 18.789063, 16.605469, 13.757813, 13.515625, 10.667969);
            cr.CurveTo(10.394531, 7.582031, 5.398438, 7.582031, 2.3125, 10.667969);
            cr.ClosePath();
            cr.MoveTo(2.3125, 10.667969);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(136.710938, 145.0625);
            cr.CurveTo(133.621094, 148.160156, 133.621094, 153.183594, 136.710938, 156.273438);
            cr.CurveTo(138.253906, 157.820313, 140.273438, 158.609375, 142.296875, 158.605469);
            cr.CurveTo(144.316406, 158.605469, 146.371094, 157.816406, 147.917969, 156.273438);
            cr.CurveTo(151.007813, 153.183594, 151.007813, 148.15625, 147.917969, 145.0625);
            cr.CurveTo(144.828125, 141.980469, 139.800781, 141.980469, 136.710938, 145.0625);
            cr.ClosePath();
            cr.MoveTo(136.710938, 145.0625);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(47.097656, 55.496094);
            cr.CurveTo(44.007813, 58.585938, 44.007813, 63.574219, 47.097656, 66.699219);
            cr.CurveTo(48.644531, 68.246094, 50.664063, 68.996094, 52.722656, 68.996094);
            cr.CurveTo(54.742188, 68.996094, 56.757813, 68.246094, 58.304688, 66.699219);
            cr.CurveTo(61.394531, 63.578125, 61.394531, 58.585938, 58.304688, 55.496094);
            cr.CurveTo(55.214844, 52.410156, 50.1875, 52.410156, 47.097656, 55.496094);
            cr.ClosePath();
            cr.MoveTo(47.097656, 55.496094);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(69.511719, 77.867188);
            cr.CurveTo(66.425781, 80.957031, 66.425781, 85.984375, 69.511719, 89.078125);
            cr.CurveTo(71.058594, 90.617188, 73.078125, 91.414063, 75.09375, 91.410156);
            cr.CurveTo(77.117188, 91.410156, 79.136719, 90.617188, 80.722656, 89.078125);
            cr.CurveTo(83.804688, 85.984375, 83.804688, 80.957031, 80.722656, 77.867188);
            cr.CurveTo(77.589844, 74.78125, 72.601563, 74.78125, 69.511719, 77.867188);
            cr.ClosePath();
            cr.MoveTo(69.511719, 77.867188);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(226.320313, 234.675781);
            cr.CurveTo(223.191406, 237.769531, 223.191406, 242.796875, 226.320313, 245.882813);
            cr.CurveTo(227.867188, 247.429688, 229.882813, 248.183594, 231.902344, 248.183594);
            cr.CurveTo(233.921875, 248.183594, 235.941406, 247.429688, 237.488281, 245.882813);
            cr.CurveTo(240.574219, 242.792969, 240.574219, 237.769531, 237.488281, 234.675781);
            cr.CurveTo(234.394531, 231.589844, 229.40625, 231.589844, 226.320313, 234.675781);
            cr.ClosePath();
            cr.MoveTo(226.320313, 234.675781);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(248.691406, 257.089844);
            cr.CurveTo(245.609375, 260.179688, 245.609375, 265.167969, 248.691406, 268.296875);
            cr.CurveTo(250.242188, 269.84375, 252.257813, 270.597656, 254.316406, 270.597656);
            cr.CurveTo(256.335938, 270.597656, 258.351563, 269.84375, 259.902344, 268.296875);
            cr.CurveTo(262.992188, 265.167969, 262.992188, 260.175781, 259.902344, 257.089844);
            cr.CurveTo(256.808594, 254.003906, 251.78125, 254.003906, 248.691406, 257.089844);
            cr.ClosePath();
            cr.MoveTo(248.691406, 257.089844);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(282.308594, 279.503906);
            cr.CurveTo(279.226563, 276.378906, 274.191406, 276.378906, 271.105469, 279.503906);
            cr.CurveTo(268.015625, 282.59375, 268.015625, 287.585938, 271.105469, 290.667969);
            cr.CurveTo(272.648438, 292.222656, 274.671875, 293.003906, 276.6875, 293.003906);
            cr.CurveTo(278.75, 293.003906, 280.773438, 292.222656, 282.308594, 290.667969);
            cr.CurveTo(285.398438, 287.589844, 285.398438, 282.59375, 282.308594, 279.503906);
            cr.ClosePath();
            cr.MoveTo(282.308594, 279.503906);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(203.90625, 212.265625);
            cr.CurveTo(200.824219, 215.394531, 200.824219, 220.382813, 203.90625, 223.472656);
            cr.CurveTo(205.453125, 225.019531, 207.472656, 225.808594, 209.5, 225.808594);
            cr.CurveTo(211.511719, 225.808594, 213.570313, 225.019531, 215.121094, 223.472656);
            cr.CurveTo(218.203125, 220.378906, 218.203125, 215.394531, 215.121094, 212.265625);
            cr.CurveTo(212.023438, 209.175781, 206.996094, 209.175781, 203.90625, 212.265625);
            cr.ClosePath();
            cr.MoveTo(203.90625, 212.265625);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(181.492188, 189.890625);
            cr.CurveTo(178.40625, 192.984375, 178.40625, 197.96875, 181.492188, 201.101563);
            cr.CurveTo(183.039063, 202.640625, 185.0625, 203.394531, 187.121094, 203.390625);
            cr.CurveTo(189.136719, 203.390625, 191.160156, 202.640625, 192.703125, 201.101563);
            cr.CurveTo(195.789063, 197.96875, 195.789063, 192.980469, 192.703125, 189.890625);
            cr.CurveTo(189.613281, 186.804688, 184.582031, 186.804688, 181.492188, 189.890625);
            cr.ClosePath();
            cr.MoveTo(181.492188, 189.890625);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(159.121094, 167.480469);
            cr.CurveTo(155.992188, 170.570313, 155.992188, 175.597656, 159.121094, 178.691406);
            cr.CurveTo(160.667969, 180.230469, 162.6875, 180.984375, 164.703125, 180.984375);
            cr.CurveTo(166.722656, 180.984375, 168.742188, 180.230469, 170.289063, 178.691406);
            cr.CurveTo(173.375, 175.597656, 173.375, 170.570313, 170.289063, 167.480469);
            cr.CurveTo(167.199219, 164.394531, 162.210938, 164.394531, 159.121094, 167.480469);
            cr.ClosePath();
            cr.MoveTo(159.121094, 167.480469);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(162.050781, 84.839844);
            cr.CurveTo(158.96875, 87.925781, 158.96875, 92.957031, 162.050781, 96.042969);
            cr.CurveTo(163.59375, 97.589844, 165.617188, 98.34375, 167.640625, 98.34375);
            cr.CurveTo(169.660156, 98.34375, 171.714844, 97.589844, 173.261719, 96.042969);
            cr.CurveTo(176.347656, 92.957031, 176.347656, 87.925781, 173.261719, 84.839844);
            cr.CurveTo(170.171875, 81.75, 165.144531, 81.75, 162.050781, 84.839844);
            cr.ClosePath();
            cr.MoveTo(162.050781, 84.839844);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(184.464844, 107.210938);
            cr.CurveTo(181.339844, 110.339844, 181.339844, 115.332031, 184.464844, 118.417969);
            cr.CurveTo(186.007813, 119.960938, 188.03125, 120.753906, 190.046875, 120.753906);
            cr.CurveTo(192.0625, 120.753906, 194.085938, 119.960938, 195.632813, 118.417969);
            cr.CurveTo(198.714844, 115.332031, 198.714844, 110.339844, 195.632813, 107.210938);
            cr.CurveTo(192.542969, 104.125, 187.550781, 104.125, 184.464844, 107.210938);
            cr.ClosePath();
            cr.MoveTo(184.464844, 107.210938);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(139.640625, 62.425781);
            cr.CurveTo(136.554688, 65.519531, 136.554688, 70.546875, 139.640625, 73.628906);
            cr.CurveTo(141.1875, 75.175781, 143.203125, 75.925781, 145.261719, 75.925781);
            cr.CurveTo(147.28125, 75.925781, 149.300781, 75.175781, 150.847656, 73.628906);
            cr.CurveTo(153.929688, 70.542969, 153.929688, 65.515625, 150.847656, 62.425781);
            cr.CurveTo(147.753906, 59.335938, 142.730469, 59.335938, 139.640625, 62.425781);
            cr.ClosePath();
            cr.MoveTo(139.640625, 62.425781);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(117.226563, 40.011719);
            cr.CurveTo(114.144531, 43.140625, 114.144531, 48.132813, 117.226563, 51.21875);
            cr.CurveTo(118.816406, 52.761719, 120.832031, 53.558594, 122.855469, 53.558594);
            cr.CurveTo(124.871094, 53.558594, 126.890625, 52.765625, 128.4375, 51.21875);
            cr.CurveTo(131.523438, 48.128906, 131.523438, 43.140625, 128.4375, 40.011719);
            cr.CurveTo(125.34375, 36.925781, 120.355469, 36.925781, 117.226563, 40.011719);
            cr.ClosePath();
            cr.MoveTo(117.226563, 40.011719);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(206.839844, 129.621094);
            cr.CurveTo(203.75, 132.714844, 203.75, 137.746094, 206.839844, 140.832031);
            cr.CurveTo(208.378906, 142.375, 210.402344, 143.167969, 212.464844, 143.167969);
            cr.CurveTo(214.476563, 143.167969, 216.5, 142.375, 218.046875, 140.832031);
            cr.CurveTo(221.132813, 137.742188, 221.132813, 132.710938, 218.046875, 129.621094);
            cr.CurveTo(214.953125, 126.535156, 209.925781, 126.535156, 206.839844, 129.621094);
            cr.ClosePath();
            cr.MoveTo(206.839844, 129.621094);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(251.660156, 174.453125);
            cr.CurveTo(248.578125, 177.535156, 248.578125, 182.527344, 251.660156, 185.621094);
            cr.CurveTo(253.207031, 187.160156, 255.230469, 187.953125, 257.246094, 187.953125);
            cr.CurveTo(259.269531, 187.953125, 261.289063, 187.160156, 262.871094, 185.621094);
            cr.CurveTo(265.960938, 182.527344, 265.960938, 177.539063, 262.871094, 174.453125);
            cr.CurveTo(259.742188, 171.324219, 254.753906, 171.324219, 251.660156, 174.453125);
            cr.ClosePath();
            cr.MoveTo(251.660156, 174.453125);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(330.035156, 241.648438);
            cr.CurveTo(326.945313, 238.519531, 321.949219, 238.519531, 318.863281, 241.648438);
            cr.CurveTo(315.777344, 244.738281, 315.777344, 249.722656, 318.863281, 252.816406);
            cr.CurveTo(320.410156, 254.355469, 322.425781, 255.152344, 324.449219, 255.152344);
            cr.CurveTo(326.464844, 255.152344, 328.484375, 254.355469, 330.035156, 252.816406);
            cr.CurveTo(333.15625, 249.722656, 333.15625, 244.738281, 330.035156, 241.648438);
            cr.ClosePath();
            cr.MoveTo(330.035156, 241.648438);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(296.453125, 219.230469);
            cr.CurveTo(293.359375, 222.328125, 293.359375, 227.351563, 296.453125, 230.441406);
            cr.CurveTo(297.996094, 231.988281, 300.015625, 232.738281, 302.03125, 232.734375);
            cr.CurveTo(304.089844, 232.734375, 306.109375, 231.988281, 307.65625, 230.441406);
            cr.CurveTo(310.746094, 227.351563, 310.746094, 222.324219, 307.65625, 219.230469);
            cr.CurveTo(304.5625, 216.148438, 299.539063, 216.148438, 296.453125, 219.230469);
            cr.ClosePath();
            cr.MoveTo(296.453125, 219.230469);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(274.03125, 196.824219);
            cr.CurveTo(270.953125, 199.914063, 270.953125, 204.945313, 274.03125, 208.03125);
            cr.CurveTo(275.582031, 209.570313, 277.601563, 210.367188, 279.660156, 210.367188);
            cr.CurveTo(281.679688, 210.367188, 283.699219, 209.570313, 285.246094, 208.03125);
            cr.CurveTo(288.328125, 204.941406, 288.328125, 199.910156, 285.246094, 196.824219);
            cr.CurveTo(282.15625, 193.734375, 277.125, 193.734375, 274.03125, 196.824219);
            cr.ClosePath();
            cr.MoveTo(274.03125, 196.824219);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(229.253906, 152.039063);
            cr.CurveTo(226.164063, 155.128906, 226.164063, 160.121094, 229.253906, 163.242188);
            cr.CurveTo(230.796875, 164.789063, 232.816406, 165.539063, 234.839844, 165.542969);
            cr.CurveTo(236.859375, 165.542969, 238.914063, 164.789063, 240.460938, 163.242188);
            cr.CurveTo(243.546875, 160.121094, 243.546875, 155.125, 240.460938, 152.039063);
            cr.CurveTo(237.367188, 148.945313, 232.339844, 148.945313, 229.253906, 152.039063);
            cr.ClosePath();
            cr.MoveTo(229.253906, 152.039063);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(91.804688, 185.574219);
            cr.CurveTo(88.71875, 188.664063, 88.71875, 193.65625, 91.804688, 196.78125);
            cr.CurveTo(93.351563, 198.332031, 95.367188, 199.078125, 97.390625, 199.078125);
            cr.CurveTo(99.410156, 199.078125, 101.460938, 198.328125, 103.011719, 196.78125);
            cr.CurveTo(106.101563, 193.65625, 106.101563, 188.664063, 103.011719, 185.574219);
            cr.CurveTo(99.921875, 182.488281, 94.894531, 182.488281, 91.804688, 185.574219);
            cr.ClosePath();
            cr.MoveTo(91.804688, 185.574219);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(114.179688, 207.953125);
            cr.CurveTo(111.089844, 211.078125, 111.089844, 216.070313, 114.179688, 219.15625);
            cr.CurveTo(115.757813, 220.699219, 117.78125, 221.492188, 119.800781, 221.492188);
            cr.CurveTo(121.824219, 221.492188, 123.839844, 220.699219, 125.382813, 219.15625);
            cr.CurveTo(128.476563, 216.070313, 128.476563, 211.078125, 125.382813, 207.953125);
            cr.CurveTo(122.296875, 204.863281, 117.304688, 204.863281, 114.179688, 207.953125);
            cr.ClosePath();
            cr.MoveTo(114.179688, 207.953125);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(47.019531, 140.753906);
            cr.CurveTo(43.894531, 143.878906, 43.894531, 148.871094, 47.019531, 151.960938);
            cr.CurveTo(48.566406, 153.503906, 50.585938, 154.296875, 52.605469, 154.296875);
            cr.CurveTo(54.617188, 154.296875, 56.640625, 153.503906, 58.191406, 151.960938);
            cr.CurveTo(61.273438, 148.871094, 61.273438, 143.882813, 58.191406, 140.753906);
            cr.CurveTo(55.101563, 137.660156, 50.105469, 137.660156, 47.019531, 140.753906);
            cr.ClosePath();
            cr.MoveTo(47.019531, 140.753906);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(69.394531, 163.167969);
            cr.CurveTo(66.304688, 166.253906, 66.304688, 171.285156, 69.394531, 174.371094);
            cr.CurveTo(70.933594, 175.914063, 72.953125, 176.667969, 75.019531, 176.667969);
            cr.CurveTo(77.035156, 176.667969, 79.054688, 175.914063, 80.601563, 174.371094);
            cr.CurveTo(83.691406, 171.285156, 83.691406, 166.25, 80.601563, 163.167969);
            cr.CurveTo(77.511719, 160.074219, 72.480469, 160.074219, 69.394531, 163.167969);
            cr.ClosePath();
            cr.MoveTo(69.394531, 163.167969);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(136.589844, 230.363281);
            cr.CurveTo(133.503906, 233.453125, 133.503906, 238.484375, 136.589844, 241.566406);
            cr.CurveTo(138.132813, 243.113281, 140.15625, 243.90625, 142.210938, 243.90625);
            cr.CurveTo(144.230469, 243.90625, 146.25, 243.117188, 147.796875, 241.566406);
            cr.CurveTo(150.882813, 238.480469, 150.882813, 233.449219, 147.796875, 230.363281);
            cr.CurveTo(144.707031, 227.273438, 139.675781, 227.273438, 136.589844, 230.363281);
            cr.ClosePath();
            cr.MoveTo(136.589844, 230.363281);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(259.820313, 342.390625);
            cr.CurveTo(256.722656, 339.300781, 251.699219, 339.300781, 248.617188, 342.390625);
            cr.CurveTo(245.523438, 345.472656, 245.523438, 350.464844, 248.617188, 353.558594);
            cr.CurveTo(250.15625, 355.136719, 252.175781, 355.890625, 254.199219, 355.890625);
            cr.CurveTo(256.210938, 355.890625, 258.273438, 355.136719, 259.820313, 353.558594);
            cr.CurveTo(262.910156, 350.464844, 262.910156, 345.472656, 259.820313, 342.390625);
            cr.ClosePath();
            cr.MoveTo(259.820313, 342.390625);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(203.789063, 297.558594);
            cr.CurveTo(200.699219, 300.648438, 200.699219, 305.675781, 203.789063, 308.769531);
            cr.CurveTo(205.332031, 310.308594, 207.351563, 311.101563, 209.410156, 311.101563);
            cr.CurveTo(211.433594, 311.101563, 213.453125, 310.308594, 214.996094, 308.769531);
            cr.CurveTo(218.085938, 305.675781, 218.085938, 300.648438, 214.996094, 297.558594);
            cr.CurveTo(211.90625, 294.472656, 206.875, 294.472656, 203.789063, 297.558594);
            cr.ClosePath();
            cr.MoveTo(203.789063, 297.558594);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(159.007813, 252.777344);
            cr.CurveTo(155.914063, 255.863281, 155.914063, 260.890625, 159.007813, 263.980469);
            cr.CurveTo(160.550781, 265.527344, 162.566406, 266.277344, 164.589844, 266.277344);
            cr.CurveTo(166.648438, 266.277344, 168.664063, 265.527344, 170.210938, 263.980469);
            cr.CurveTo(173.300781, 260.886719, 173.300781, 255.859375, 170.210938, 252.777344);
            cr.CurveTo(167.121094, 249.683594, 162.089844, 249.683594, 159.007813, 252.777344);
            cr.ClosePath();
            cr.MoveTo(159.007813, 252.777344);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(226.203125, 319.972656);
            cr.CurveTo(223.117188, 323.058594, 223.117188, 328.089844, 226.203125, 331.175781);
            cr.CurveTo(227.75, 332.726563, 229.765625, 333.476563, 231.789063, 333.472656);
            cr.CurveTo(233.808594, 333.472656, 235.867188, 332.71875, 237.410156, 331.175781);
            cr.CurveTo(240.496094, 328.089844, 240.496094, 323.058594, 237.410156, 319.972656);
            cr.CurveTo(234.320313, 316.882813, 229.289063, 316.882813, 226.203125, 319.972656);
            cr.ClosePath();
            cr.MoveTo(226.203125, 319.972656);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(181.417969, 275.183594);
            cr.CurveTo(178.332031, 278.273438, 178.332031, 283.265625, 181.417969, 286.351563);
            cr.CurveTo(182.960938, 287.902344, 184.980469, 288.6875, 187, 288.6875);
            cr.CurveTo(189.023438, 288.6875, 191.039063, 287.898438, 192.582031, 286.351563);
            cr.CurveTo(195.714844, 283.265625, 195.714844, 278.269531, 192.582031, 275.183594);
            cr.CurveTo(189.496094, 272.058594, 184.507813, 272.058594, 181.417969, 275.183594);
            cr.ClosePath();
            cr.MoveTo(181.417969, 275.183594);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(453.265625, 282.601563);
            cr.CurveTo(430.628906, 259.769531, 402.457031, 231.355469, 367.332031, 231.355469);
            cr.CurveTo(331.660156, 231.355469, 298.582031, 257.164063, 277.117188, 278.816406);
            cr.CurveTo(252.753906, 303.390625, 249.46875, 338.570313, 249.46875, 369.753906);
            cr.CurveTo(249.46875, 403.613281, 252.691406, 438.199219, 276.054688, 461.765625);
            cr.CurveTo(297.714844, 483.617188, 330.128906, 495.648438, 367.324219, 495.648438);
            cr.CurveTo(412.574219, 495.648438, 445.050781, 486.035156, 463.855469, 467.066406);
            cr.CurveTo(487.496094, 443.222656, 504, 403.207031, 504, 369.753906);
            cr.CurveTo(504.003906, 333.78125, 474.835938, 304.351563, 453.265625, 282.601563);
            cr.ClosePath();
            cr.MoveTo(349.699219, 420.472656);
            cr.CurveTo(336.589844, 420.472656, 325.9375, 409.8125, 325.9375, 396.714844);
            cr.CurveTo(325.9375, 383.613281, 336.589844, 372.957031, 349.699219, 372.957031);
            cr.CurveTo(362.792969, 372.957031, 373.449219, 383.613281, 373.449219, 396.714844);
            cr.CurveTo(373.445313, 409.820313, 362.792969, 420.472656, 349.699219, 420.472656);
            cr.ClosePath();
            cr.MoveTo(369.484375, 337.195313);
            cr.CurveTo(360.878906, 337.195313, 353.871094, 330.191406, 353.871094, 321.574219);
            cr.CurveTo(353.871094, 312.960938, 360.878906, 305.953125, 369.484375, 305.953125);
            cr.CurveTo(378.101563, 305.953125, 385.113281, 312.960938, 385.113281, 321.574219);
            cr.CurveTo(385.113281, 330.1875, 378.101563, 337.195313, 369.484375, 337.195313);
            cr.ClosePath();
            cr.MoveTo(420.605469, 388.8125);
            cr.CurveTo(411.988281, 388.8125, 404.980469, 381.808594, 404.980469, 373.199219);
            cr.CurveTo(404.980469, 364.582031, 411.988281, 357.574219, 420.605469, 357.574219);
            cr.CurveTo(429.214844, 357.574219, 436.226563, 364.582031, 436.226563, 373.199219);
            cr.CurveTo(436.230469, 381.808594, 429.21875, 388.8125, 420.605469, 388.8125);
            cr.ClosePath();
            cr.MoveTo(420.605469, 388.8125);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }

        private static void DrawWaypointMushroom1(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;

            cr.Save();
            float w = 458;
            float h = 458;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(279.3125, 399.816406);
            cr.LineTo(280, 395.355469);
            cr.CurveTo(278.367188, 394.898438, 276.757813, 394.375, 275.171875, 393.785156);
            cr.CurveTo(263.945313, 389.636719, 253.957031, 382.304688, 246.660156, 372.527344);
            cr.CurveTo(234.910156, 356.785156, 231.234375, 336.140625, 236.824219, 317.320313);
            cr.CurveTo(243.488281, 294.867188, 256.707031, 274.777344, 273.914063, 259.769531);
            cr.CurveTo(265.332031, 259.769531, 149.101563, 259.769531, 143.757813, 259.769531);
            cr.LineTo(129.496094, 352.480469);
            cr.CurveTo(125.425781, 378.925781, 133.085938, 405.738281, 150.507813, 426.046875);
            cr.CurveTo(167.929688, 446.355469, 193.269531, 458, 220.027344, 458);
            cr.CurveTo(243.082031, 458, 265.082031, 449.34375, 281.871094, 433.960938);
            cr.CurveTo(278.46875, 423.054688, 277.53125, 411.394531, 279.3125, 399.816406);
            cr.ClosePath();
            cr.MoveTo(279.3125, 399.816406);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(369.339844, 225.503906);
            cr.CurveTo(387.257813, 225.503906, 404.351563, 216.929688, 415.070313, 202.566406);
            cr.CurveTo(425.738281, 188.269531, 429.136719, 169.363281, 424.042969, 152.199219);
            cr.CurveTo(398.410156, 65.808594, 318.085938, 0, 220.035156, 0);
            cr.CurveTo(122.105469, 0, 41.707031, 65.632813, 16.015625, 152.195313);
            cr.CurveTo(11.046875, 168.941406, 14.085938, 187.945313, 24.988281, 202.554688);
            cr.CurveTo(35.710938, 216.925781, 52.808594, 225.503906, 70.71875, 225.503906);
            cr.ClosePath();
            cr.MoveTo(220.027344, 95.179688);
            cr.CurveTo(243.835938, 95.179688, 263.121094, 114.472656, 263.121094, 138.273438);
            cr.CurveTo(263.121094, 162.078125, 243.835938, 181.371094, 220.027344, 181.371094);
            cr.CurveTo(196.222656, 181.371094, 176.933594, 162.078125, 176.933594, 138.273438);
            cr.CurveTo(176.933594, 114.472656, 196.222656, 95.179688, 220.027344, 95.179688);
            cr.ClosePath();
            cr.MoveTo(395.277344, 160.734375);
            cr.CurveTo(397.246094, 167.367188, 396.554688, 174.515625, 393.488281, 180.621094);
            cr.CurveTo(368.351563, 176.402344, 349.207031, 154.558594, 349.207031, 128.226563);
            cr.CurveTo(349.207031, 116.011719, 353.332031, 104.769531, 360.257813, 95.796875);
            cr.CurveTo(375.96875, 114.496094, 388.074219, 136.457031, 395.277344, 160.734375);
            cr.ClosePath();
            cr.MoveTo(311.28125, 54.679688);
            cr.CurveTo(306.613281, 58.765625, 300.503906, 61.25, 293.8125, 61.25);
            cr.CurveTo(279.699219, 61.25, 268.171875, 50.234375, 267.339844, 36.332031);
            cr.CurveTo(282.800781, 40.53125, 297.566406, 46.734375, 311.28125, 54.679688);
            cr.ClosePath();
            cr.MoveTo(172.714844, 36.335938);
            cr.CurveTo(171.882813, 50.234375, 160.355469, 61.25, 146.242188, 61.25);
            cr.CurveTo(139.550781, 61.25, 133.445313, 58.769531, 128.78125, 54.683594);
            cr.CurveTo(142.492188, 46.738281, 157.253906, 40.535156, 172.714844, 36.335938);
            cr.ClosePath();
            cr.MoveTo(44.785156, 160.734375);
            cr.CurveTo(52.277344, 135.488281, 65.0625, 112.746094, 81.699219, 93.570313);
            cr.CurveTo(89.710938, 102.878906, 94.5625, 114.980469, 94.5625, 128.226563);
            cr.CurveTo(94.5625, 155.75, 73.648438, 178.386719, 46.832031, 181.097656);
            cr.CurveTo(43.554688, 174.890625, 42.761719, 167.539063, 44.785156, 160.734375);
            cr.ClosePath();
            cr.MoveTo(44.785156, 160.734375);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(442.96875, 325.859375);
            cr.CurveTo(432.074219, 289.152344, 398.71875, 261.386719, 358.070313, 259.769531);
            cr.LineTo(350.496094, 259.769531);
            cr.CurveTo(309.894531, 261.386719, 276.496094, 289.109375, 265.59375, 325.855469);
            cr.CurveTo(262.683594, 335.652344, 264.59375, 346.390625, 270.707031, 354.582031);
            cr.CurveTo(276.824219, 362.777344, 286.574219, 367.667969, 296.789063, 367.667969);
            cr.LineTo(314.621094, 367.667969);
            cr.LineTo(308.972656, 404.378906);
            cr.CurveTo(306.933594, 417.617188, 310.769531, 431.035156, 319.484375, 441.195313);
            cr.CurveTo(328.203125, 451.359375, 340.882813, 457.1875, 354.277344, 457.1875);
            cr.CurveTo(367.667969, 457.1875, 380.347656, 451.359375, 389.066406, 441.195313);
            cr.CurveTo(397.785156, 431.035156, 401.617188, 417.613281, 399.582031, 404.378906);
            cr.LineTo(393.9375, 367.667969);
            cr.LineTo(411.769531, 367.667969);
            cr.CurveTo(421.988281, 367.667969, 431.738281, 362.777344, 437.851563, 354.582031);
            cr.CurveTo(443.960938, 346.394531, 445.875, 335.65625, 442.96875, 325.859375);
            cr.ClosePath();
            cr.MoveTo(442.96875, 325.859375);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }

        private static void DrawWaypointMushroom2(Context cr, int x, int y, float width, float height, IReadOnlyList<double> rgba)
        {
            var matrix = cr.Matrix;

            cr.Save();
            float w = 330;
            float h = 330;
            var scale = Math.Min(width / w, height / h);
            matrix.Translate(x + Math.Max(0, (width - w * scale) / 2), y + Math.Max(0, (height - h * scale) / 2));
            matrix.Scale(scale, scale);
            cr.Matrix = matrix;

            cr.Operator = Operator.Over;
            Pattern pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(165.089844, 0);
            cr.CurveTo(88.445313, 0, 26.316406, 57.804688, 26.316406, 129.109375);
            cr.CurveTo(26.316406, 133.203125, 26.53125, 164.066406, 26.929688, 168.066406);
            cr.CurveTo(26.929688, 168.066406, 31.101563, 185.941406, 165.089844, 185.941406);
            cr.CurveTo(300.671875, 185.941406, 303.246094, 168.066406, 303.246094, 168.066406);
            cr.CurveTo(303.648438, 164.066406, 303.863281, 133.203125, 303.863281, 129.109375);
            cr.CurveTo(303.863281, 57.804688, 241.730469, 0, 165.089844, 0);
            cr.ClosePath();
            cr.MoveTo(121.425781, 41.464844);
            cr.CurveTo(109.492188, 44.269531, 98.152344, 50.292969, 88.148438, 58.476563);
            cr.CurveTo(64.972656, 77.4375, 56.023438, 104.960938, 58.175781, 134.375);
            cr.CurveTo(58.71875, 141.789063, 47.195313, 141.738281, 46.65625, 134.375);
            cr.CurveTo(44.347656, 102.828125, 54.375, 71.300781, 79.359375, 50.855469);
            cr.CurveTo(90.539063, 41.710938, 104.144531, 33.699219, 118.363281, 30.359375);
            cr.CurveTo(125.589844, 28.660156, 128.65625, 39.769531, 121.425781, 41.464844);
            cr.ClosePath();
            cr.MoveTo(121.425781, 41.464844);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Operator = Operator.Over;
            pattern = new SolidPattern(rgba[0], rgba[1], rgba[2], rgba[3]);
            cr.SetSource(pattern);

            cr.NewPath();
            cr.MoveTo(206.988281, 194.84375);
            cr.CurveTo(193.152344, 195.359375, 181.761719, 195.621094, 165.089844, 195.621094);
            cr.CurveTo(150.199219, 195.621094, 138.433594, 195.394531, 123.191406, 194.804688);
            cr.LineTo(109.046875, 316.46875);
            cr.CurveTo(108.550781, 324.011719, 114.328125, 330.179688, 121.886719, 330.179688);
            cr.LineTo(208.292969, 330.179688);
            cr.CurveTo(215.851563, 330.179688, 221.628906, 324.011719, 221.132813, 316.46875);
            cr.ClosePath();
            cr.MoveTo(206.988281, 194.84375);
            cr.Tolerance = 0.1;
            cr.Antialias = Antialias.Default;
            cr.FillRule = FillRule.Winding;
            cr.FillPreserve();
            pattern.Dispose();

            cr.Restore();
        }
    }
}