using System;
using System.Drawing;
using Robocode;
using Robocode.Util;

namespace VDP
{
    public class FirstRobotTest : AdvancedRobot
    {
        public override bool IsAdjustGunForRobotTurn { get => base.IsAdjustGunForRobotTurn; set => base.IsAdjustGunForRobotTurn = value; }
        int walk = 50;
        double changeDirection = 90;
        double changeAhead;
        double[,] enemy_data = new Double[2, 4];
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void OnBattleEnded(BattleEndedEvent evnt)
        {
            base.OnBattleEnded(evnt);
        }
        //-------------------------------Bala certeira-------------------------------------
        public override void OnBulletHit(BulletHitEvent evnt)
        {
            Console.WriteLine("Bullet Heading: "+ evnt.Bullet.Heading+ "\nRadarHeading: "+ RadarHeading);
            if(evnt.Bullet.Heading == RadarHeading)
            {
                Scan();
            }
        }

        public override void OnBulletHitBullet(BulletHitBulletEvent evnt)
        {
            base.OnBulletHitBullet(evnt);
        }
        //-------------------------------Bala Missed-------------------------------------
        public override void OnBulletMissed(BulletMissedEvent evnt)
        {
            base.OnBulletMissed(evnt);
        }

        public override void OnDeath(DeathEvent evnt)
        {
            base.OnDeath(evnt);
        }
        //-------------------------------Bala-------------------------------------
        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            //Desvia();
        }
        //-------------------------------Robô-------------------------------------
        public override void OnHitRobot(HitRobotEvent evnt)
        {
            TurnGunRight(Utils.NormalRelativeAngleDegrees(Heading + evnt.Bearing - GunHeading));
            FireBullet(Rules.MAX_BULLET_POWER);
        }
        //-------------------------------Muro-------------------------------------
        public override void OnHitWall(HitWallEvent evnt)
        {
            /*TurnLeft(180);
            Ahead(50);
            TurnRadarRight(360);*/
            //Back(5);
            //if(evnt.Bearing == )
            SetTurnRight(Utils.NormalRelativeAngleDegrees(180 + evnt.Bearing));
            SetTurnRadarRight(360);
            Execute();
            //Desvia();
        }

        public override void OnKeyPressed(KeyEvent e)
        {
            base.OnKeyPressed(e);
        }

        public override void OnKeyReleased(KeyEvent e)
        {
            base.OnKeyReleased(e);
        }

        public override void OnKeyTyped(KeyEvent e)
        {
            base.OnKeyTyped(e);
        }

        public override void OnMouseClicked(MouseEvent e)
        {
            base.OnMouseClicked(e);
        }

        public override void OnMouseDragged(MouseEvent e)
        {
            base.OnMouseDragged(e);
        }

        public override void OnMouseEntered(MouseEvent e)
        {
            base.OnMouseEntered(e);
        }

        public override void OnMouseExited(MouseEvent e)
        {
            base.OnMouseExited(e);
        }

        public override void OnMouseMoved(MouseEvent e)
        {
            base.OnMouseMoved(e);
        }

        public override void OnMousePressed(MouseEvent e)
        {
            base.OnMousePressed(e);
        }

        public override void OnMouseReleased(MouseEvent e)
        {
            base.OnMouseReleased(e);
        }

        public override void OnMouseWheelMoved(MouseWheelMovedEvent e)
        {
            base.OnMouseWheelMoved(e);
        }

        public override void OnPaint(IGraphics graphics)
        {
            base.OnPaint(graphics);
        }

        public override void OnRobotDeath(RobotDeathEvent evnt)
        {
            base.OnRobotDeath(evnt);
        }

        public override void OnRoundEnded(RoundEndedEvent evnt)
        {
            base.OnRoundEnded(evnt);
        }

        public void Atira(ScannedRobotEvent evnt)
        {
            /*if (Energy > 60)
            {
                if(evnt.Distance > 130)
                {
                    base.FireBullet(1);
                }else if(evnt.Distance > 80)
                {
                    FireBullet(2);
                }else if(evnt.Distance < 20)
                {
                    FireBullet(3);
                }
            }
            else
            {
                base.FireBullet(1);
            }*/
            if(evnt.Energy <= 16)
            {
                Ahead(evnt.Distance);
            }
            if(GunHeat == 0 && Math.Abs(RadarTurnRemaining) < 1 && evnt.Distance < 100)
            {
                Fire(Math.Min(400 / evnt.Distance, 3));
            }
            Scan();
        }

        public void Desvia()
        {
            SetTurnRight(Utils.NormalRelativeAngleDegrees(90 + (Heading - RadarHeading)));
            SetAhead(walk);
            walk *= -1;
            Scan();
            Execute();
            //TurnRadarRight(45);
            //TurnRadarLeft(90);
        }
        public void updateEnemy(double time, double energy, double distance, double velocity)
        {
            if(time != enemy_data[0, 0])
            {
                enemy_data[1, 0] = enemy_data[0, 0];
                enemy_data[1, 1] = enemy_data[0, 1];
                enemy_data[1, 2] = enemy_data[0, 2];
                enemy_data[1, 3] = enemy_data[0, 3];
                enemy_data[0, 0] = time;
                enemy_data[0, 1] = energy;
                enemy_data[0, 2] = distance;
                enemy_data[0, 3] = velocity;
            }
        }
        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            SetTurnRadarRight(Utils.NormalRelativeAngleDegrees(Heading + evnt.Bearing - RadarHeading));
            SetTurnGunRight(Utils.NormalRelativeAngleDegrees(Heading + evnt.Bearing - GunHeading));
            updateEnemy(evnt.Time, evnt.Energy, evnt.Distance, evnt.Velocity);
            if(enemy_data[0, 2] < enemy_data[1, 2] && enemy_data[0, 3] == enemy_data[1, 3])
            {
                Fire(3);
            }
            if(evnt.Distance > 100)
            {
                SetTurnRight(Utils.NormalRelativeAngleDegrees(evnt.Bearing));
                do
                {
                    SetAhead(100);
                } while (evnt.Distance == 10);
                Execute();
                /*if(Heading == 90)
                {
                    TurnLeft(90);
                    Ahead(50);
                }
                else if(Heading == 270)
                {
                    TurnRight(90);
                    Ahead(50);
                }else if(Heading > 0 && Heading < 90)
                {
                    TurnRight
                    Ahead();
                }*/
            }
            if(enemy_data[0, 1] < enemy_data[1, 1] && (enemy_data[0, 1] - enemy_data[1, 1]) < 3 && evnt.Distance > 200)
            {
                //TurnGunRight(Utils.NormalRelativeAngleDegrees(Heading + evnt.Bearing - GunHeading));
                TurnRight(Utils.NormalRelativeAngleDegrees(evnt.Bearing + changeDirection));
                changeDirection *= -1;
                /*if (evnt.Distance <= 200)//40
                {
                    SetTurnGunRight(Utils.NormalRelativeAngleDegrees(evnt.Bearing));
                    Fire(1);
                    Execute();
                }
                else
                {
                }*/
                changeAhead = 50;
                SetAhead(changeAhead);
                Execute();
                Scan();
            }
            else
            {
                Atira(evnt);
            }
            Execute();
            /*quantRobots = Others;
            if(quantRobots > 2)
            {
                Atira();
            }
            else
            {
                Atira();
            }*/
            //Console.WriteLine("Gun Heading: " + GunHeading+ "\nRadar Heading: "+ RadarHeading);
            //Console.WriteLine(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
            /*if (RadarTurnRemaining > 0)
            {
                if(GunTurnRemaining > 0)
                {
                    if (RadarHeading >= 0 && RadarHeading < 90)
                    {
                        if (GunHeading >= 0 && GunHeading < 90)
                        {
                            if(GunHeading > RadarHeading)
                            {
                                TurnGunRight(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                            }
                            else
                            {
                                TurnGunLeft(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                            }
                        }
                    }
                    else
                    {
                        if (GunHeading >= 270 && GunHeading < 360)
                        {
                            if (GunHeading > RadarHeading)
                            {
                                TurnGunRight(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                            }
                            else
                            {
                                TurnGunLeft(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                            }
                        }
                    }
                }
                else
                {
                    TurnGunRight(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                }
            }
            else
            {
                if (GunTurnRemaining < 0)
                {
                    if (RadarHeading >= 90 && RadarHeading < 180)
                    {
                        if (GunHeading >= 90 && GunHeading < 180)
                        {
                            if (GunHeading > RadarHeading)
                            {
                                TurnGunRight(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                            }
                            else
                            {
                                TurnGunLeft(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                            }
                        }
                    }
                    else
                    {
                        if (GunHeading >= 180 && GunHeading < 270)
                        {
                            if (GunHeading > RadarHeading)
                            {
                                TurnGunRight(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                            }
                            else
                            {
                                TurnGunLeft(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                            }
                        }
                    }
                }
                else
                {
                     TurnGunRight(Utils.NormalAbsoluteAngleDegrees(GunHeading - RadarHeading));
                }
            }*/
            /*if (IsAdjustRadarForGunTurn)
            {
                TurnGunLeft(20);
            }*/

            //Scan();
            //IsAdjustRadarForGunTurn = true;
            //TurnRadarLeft(20);

            /*if (Heading.Equals("North") && evnt.Bearing.Equals("South") || Heading.Equals("West") && evnt.Bearing.Equals("East") )
            {
                TurnLeft(90);
                Ahead(50);
            }else if(Heading.Equals("South") && evnt.Bearing.Equals("North") || Heading.Equals("East") && evnt.Bearing.Equals("West"))
            {
                TurnRight(90);
                Ahead(50);
            }*/
        }

        public override void OnStatus(StatusEvent e)
        {
            base.OnStatus(e);
        }

        public override void OnWin(WinEvent evnt)
        {
            TurnRight(36000);
        }

        public override void Run()
        {
            //TurnLeft(Heading - 90);
            //TurnGunRight(90);
            //int i = 50, movA = 0, movB = 0;
            //TurnGunRight(360);
            SetColors(Color.DarkRed, Color.Black, Color.Gray);
            IsAdjustGunForRobotTurn = true;
            IsAdjustRadarForGunTurn = true;
            enemy_data[0, 0] = 0;
            enemy_data[0, 1] = 0;
            enemy_data[0, 2] = 0;
            enemy_data[0, 3] = 0;
            enemy_data[1, 0] = 0;
            enemy_data[1, 1] = 0;
            enemy_data[1, 2] = 0;
            enemy_data[1, 3] = 0;

            while (true)
            {
                TurnRadarRight(360);
                //Desvia();
                /*//TurnRight(90);
                if (movA < 150)
                {
                    //movA = movA + i;
                    //Ahead(movA);
                    TurnGunRight(360);
                }
                else
                {
                    //movA = 50;
                    //Ahead(movA);
                }
                //TurnLeft(90);
                if (movB < 150)
                {
                    //movB = movB + i;
                    //Ahead(movB);
                    //TurnGunRight(360);
                }
                else
                {
                    //movB = 50;
                    //Ahead(movB);
                }*/
            }
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
