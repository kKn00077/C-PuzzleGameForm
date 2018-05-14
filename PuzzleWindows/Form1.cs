using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleWindows
{
    public partial class Form1 : Form
    {
        PuzzleGameEngine pge;
        List<Image> imgList = new List<Image>(); //Java의 ArrayList와 동일
        private int puzzleSize = 16;
        int imgWidth = 100;
        int imgHeight = 100;
        private Font theFont;
        private Brush theBrush;
        private int theTick;
        private int theGameTick; // 기준
        private Pen thePen;

        public Form1()
        {
            InitializeComponent();

            //image를 가져오기
            //image를 List에 삽입
            for(int i = 0; i < puzzleSize; i++)
            {
                //string fileName = "pic_" + (char)('a' + i) + ".png";
                string fileName = string.Format("pic_{0}.png",(char)('a' + i));
                Image tmpI = Image.FromFile(fileName);//C:\Users\Mirim\source\repos\PuzzleWindows\PuzzleWindows\bin\Debug 에 이미지 파일을 넣어줘야함. 왜냐면 실행이 Debug에서 실행되기 때문에 리소스 파일이 Debug 안에 존재해야함
                imgList.Add(tmpI);
            }

            //PuzzleGameEngine 생성
            pge = new PuzzleGameEngine();

            // Timer
            theFont = new Font("굴림", 15);
            theBrush = new SolidBrush(Color.Green);
            thePen = new Pen(Color.Red);
            theTick = 0;
            theGameTick = 0;
            timer1.Start();
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)//Drow 이벤트, 이미지 그리기
        {
            //time 표시
            int time = 100/*제한시간*/-(theTick-theGameTick)/(1000/timer1.Interval);
            string timeString = string.Format("Time : {0:D3}", time);
            e.Graphics.DrawString(timeString, theFont, theBrush, 0, 10);
            //e.Graphics.DrawRectangle(thePen, 0, 0, time, 10);
            e.Graphics.FillRectangle(theBrush, 0, 0, time, 10);

            //time이 0이면 게임오버, 그러나 화면에는 1일때 끝나기 때문에 -1로 설정
            if (time == -1) //0을 했을 경우 : 더블버퍼를 체크해서 타임을 그리는 함수를 호출해도 버퍼때문에 별도로 할당된 메모리에만 그려두고 정작 화면에는 그리지 않은 상태임. 그려지기 전에 time이 0으로 판단되어서 1로 보이고 끝남, 더블버퍼 체크 안하면 함수 호출과 동시에 그림을 그림
            {
                timer1.Stop();
                MessageBox.Show("게임오버");
                return;
            }

            //그릴 때, 이미지를 4*4로 그리기
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (pge.GetViewIndex(i + j * 4) != puzzleSize - 1) //15번째에 있는 그림(즉 'P')은 그리지 말고 나머지는 그려라
                    {
                        e.Graphics.DrawImage(imgList[/*i + j * 4 = ABCD...순서대로 출력*/pge.GetViewIndex(i + j * 4)/*중복되지 않게 pge 안의 다른 로직를 통해 해당 메서드에서 랜덤 인덱스 리턴*/], 50 + i * imgWidth, 50 + j * imgHeight, imgWidth, imgHeight);
                    }
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int tmpX = e.X;
            int tmpY = e.Y;
            //MessageBox.Show(tmpX + ", " + tmpY);

            //x, y 좌표를 구역으로 바꾸기

            if (!((50 <= tmpX && tmpX <= 4 * imgWidth + 50) && (50 <= tmpY && tmpY <= 4 * imgHeight + 50))) return;
            tmpX -= 50;
            tmpY -= 50;
            tmpX /= imgWidth;
            tmpY /= imgHeight;
            int index = tmpX + tmpY * 4;

            //클릭했을 때, 클릭한 index와 빈 칸 index를 교체
            pge.Change(index);
            Invalidate();

            //다 맞췄으면, 메시지 박스 ㅊㅋㅊㅋ
            //창 닫기
            if (pge.isEnd())
            {
                MessageBox.Show("축하합니다!");
                Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            theTick++;
            Invalidate(); // 다시 그리는, 갱신하는 함수. 호출과 동시에 그리진 않음. 버퍼에 다녀오기 때문에 조금 텀이 존재함.
        }
    }
}
