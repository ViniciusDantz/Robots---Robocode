using System;
using Robocode;
using Robocode.Util;
using System.Drawing;

namespace VDP
{
	class Droid_Mr_Robot : TeamRobot, IDroid
    {
        public override void Run()
        {
            while (true)
            {
                TurnRadarRight(360);//radar gira 360 graus se não estiver travado em um inimigo
            }
        }
	}
}
