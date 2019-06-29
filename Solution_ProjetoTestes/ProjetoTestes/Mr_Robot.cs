using System;
using System.Drawing;
using Robocode.Util;
using Robocode;

namespace VDP
{
    class Mr_Robot : TeamRobot
    {
        public override bool IsAdjustGunForRobotTurn { get => base.IsAdjustGunForRobotTurn; set => base.IsAdjustGunForRobotTurn = value; }
        //changeDirection --> Variável usada como valor para desviar da bala do inimigo, varia entre 90 e -90, para ir para direita ou para a esquerda
        double changeDirection = 90;
        //enemy_data --> Dados do inimigo em uma matriz: cada coluna representa uma variável e as linhas representam o turno anterior e o turno presente
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
        public override void OnBulletHit(BulletHitEvent evnt)
        {
            base.OnBulletHit(evnt);
        }

        public override void OnBulletHitBullet(BulletHitBulletEvent evnt)
        {
            base.OnBulletHitBullet(evnt);
        }
        public override void OnBulletMissed(BulletMissedEvent evnt)
        {
            base.OnBulletMissed(evnt);
        }

        public override void OnDeath(DeathEvent evnt)
        {
            base.OnDeath(evnt);
        }
        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            base.OnHitByBullet(evnt);
        }
        public override void OnHitRobot(HitRobotEvent evnt)
        {
            /*
             *Método destinado para virar a arma e atirar em força máxima sempre que bater em outro robô
             */
            if (IsTeammate(evnt.Name) == false)
            {
                TurnGunRight(Utils.NormalRelativeAngleDegrees(Heading + evnt.Bearing - GunHeading));
                FireBullet(Rules.MAX_BULLET_POWER);
            }
        }
        public override void OnHitWall(HitWallEvent evnt)
        {
            /*
             * Método destinado a girar o robô para o lado contrário à parede e procurar o robô inimigo mais próximo
             * para focar o radar e segui-lo
             */
            SetTurnRight(Utils.NormalRelativeAngleDegrees(180 + evnt.Bearing));
            SetTurnRadarRight(360);
            Execute();
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
            /*
             * Método destinado a executar a ação do tiro baseando seu parâmetro
             * no inimigo escaneado e com base em seus dados é feita a decisão de atirar
             * na força proporcional à distância entre o meu robô e o inimigo, além de bater no inimigo
             * ativando o método OnHitRobot se a energia do outro for igual ou menor que 16(morte instantânea)
             */
            if (evnt.Energy <= 16)
            {
                SetTurnLeft(Utils.NormalRelativeAngleDegrees(evnt.Bearing));
                SetAhead(evnt.Distance);
                Execute();
            }
            if (GunHeat == 0 && Math.Abs(RadarTurnRemaining) < 1 && evnt.Distance < 100)
            {
                FireBullet(Math.Min(400 / evnt.Distance, 3));
            }
            Scan();
        }

        public void updateEnemy(double time, double energy, double distance, double velocity)
        {
            /*
             * Atualiza a matriz com os dados coletados do inimigo, tudo que já foi
             * coletado passa a fazer parte da linha 2, o que é novo fica na linha 1 e 
             * o que já existia é descartado
             */
            if (time != enemy_data[0, 0])
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
            /*
             * Método usado para focar o radar e a arma no inimigo;
             * atualizar seus dados na matriz enemy_data;
             * atirar bala se o inimigo estiver parado ou mais perto e com a velocidade constante
             * ou com distância menor que 22 pixels;
             * seguir o inimigo de perto;
             * desviar de balas que estejam a mais de 199 pixels de distância;
             * chamar o método Atira().
             */
            if (IsTeammate(evnt.Name) == false)
            {
                SetTurnRadarRight(Utils.NormalRelativeAngleDegrees(Heading + evnt.Bearing - RadarHeading));
                SetTurnGunRight(Utils.NormalRelativeAngleDegrees(Heading + evnt.Bearing - GunHeading));
                updateEnemy(evnt.Time, evnt.Energy, evnt.Distance, evnt.Velocity);
                if (enemy_data[0, 2] < enemy_data[1, 2] && enemy_data[0, 3] == enemy_data[1, 3] || evnt.Distance <= 22)
                {
                    FireBullet(3);
                }
                if (evnt.Distance > 100)
                {
                    SetTurnRight(Utils.NormalRelativeAngleDegrees(evnt.Bearing));
                    do
                    {
                        SetAhead(100);
                    } while (evnt.Distance == 10);
                    Execute();
                }
                if (enemy_data[0, 1] < enemy_data[1, 1] && (enemy_data[0, 1] - enemy_data[1, 1]) < 3 && evnt.Distance > 200)
                {
                    TurnRight(Utils.NormalRelativeAngleDegrees(evnt.Bearing + changeDirection));
                    changeDirection *= -1;
                    SetAhead(50);
                    Execute();
                    Scan();
                }
                else
                {
                    Atira(evnt);
                }
                Execute();
            }
        }

        public override void OnStatus(StatusEvent e)
        {
            base.OnStatus(e);
        }

        public override void OnWin(WinEvent evnt)
        {
            TurnRight(36000);//Girar muito se ganhar
        }

        public override void Run()
        {
            SetColors(Color.DarkRed, Color.Black, Color.Gray);
            //Definições para desvincular a arma do radar e do centro do robô
            IsAdjustGunForRobotTurn = true;
            IsAdjustRadarForGunTurn = true;
            //Definições da matriz com tudo zerado no começo da partida
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
                TurnRadarRight(360);//radar gira 360 graus se não estiver travado em um inimigo
            }
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}