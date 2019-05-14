/*
 * 2019-03-20
 * 
 * 디자인 정의
 * 
 * 여기서 대부분의 행위가 일어난다.
 * 
 * 작성자 서지민
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sequence_Data_Predict
{
    public partial class Main_Form : Form
    {
        string fath;
        Main_Logic CPU;

        int Result_Count = 0;
        public Main_Form()
        {
            CPU = new Main_Logic();
            InitializeComponent();
        }


        /*************************************************************************
         *        주 함수
         * ********************************************************************/

        //이제 여기서 파일도 읽고 학습도 한다.
        private void Learning_Button_Click(object sender, EventArgs e)
        {
            string fath = ShowFileOpenDialog();

            string Message = "";
            string Caption = "";

            //파일이 잘못 되었습니다.
            if(fath.Equals("-1"))
            {
                Message = "파일경로나 이름이 잘못 되었습니다.";
                Caption = "오류";
            }

            //정상
            else
            {
                ResultGrid.Columns.Clear();
                ResultGrid.Rows.Clear();
                Result_Count = 0;
                Console.WriteLine("경로 " + fath);
                /*
                 * 1. 데이터 읽기
                 * 2. 학습하기
                 */
                CPU.Set_Fath(fath);
                this.fath = fath;

                //데이터 읽기
                string Data_Stream = CPU.Read_File(fath);

                //데이터 파싱
               string[] Data_Step = CPU.Input_Data(Data_Stream);

                List<string> Data_List = new List<string>();

                for(int i =0; i < Data_Step.Length; i++)
                {
                    Data_List.Add(Data_Step[i]);
                }

                /*
                int Epoch = 50000;
                double MAX_Error = 9999;
                int Window = 5;

                try
                {
                    Epoch = int.Parse(MAX_Epoch_Text.Text.ToString().Trim());
                }
                catch(Exception exception)
                {
                    Message = "최대 학습횟수: 정수가 아닙니다. " + exception;
                    Caption = "데이터 입력오류";

                    Show_POP(Message, Caption);

                    return;
                }

                try
                {
                    MAX_Error = double.Parse(MAX_Cost_Text.Text.ToString().Trim());
                }
                catch (Exception exception)
                {
                    Message = "오차 허용치: 실수가 아닙니다. " + exception;
                    Caption = "데이터 입력오류";

                    Show_POP(Message, Caption);

                    return;
                }

                try
                {
                    Window = int.Parse(Windows_Text.Text.ToString().Trim());
                }
                catch (Exception exception)
                {
                    Message = "관측범위: 정수가 아닙니다. " + exception;
                    Caption = "데이터 입력오류";

                    Show_POP(Message, Caption);

                    return;
                }

                Console.WriteLine(Epoch);
                Console.WriteLine(MAX_Error);
                Console.WriteLine(Window);
               
                //학습하기
                CPU.Learning(Data_Step, Epoch, MAX_Error, Window);
                */

                //마르코프 학습
                CPU.Learning(Data_List);

                List<string> Keys = CPU.Get_Keys();

                ResultGrid.Columns.Clear();

                for (int i = 0; i < Keys.Count; i++)
                {
                     ResultGrid.Columns.Add(Keys[i].Trim() + "_Columns", Keys[i].Trim());
                }


                Message = "학습완료";
                Caption = "학습상태";
            }
            Show_POP(Message, Caption);
        }

        //여기서 예측한다.
        private void Predict_Button_Click(object sender, EventArgs e)
        {
            string Message = "아직 학습이 되지 않았습니다.";
            string Caption = "오류발생";

            if (!CPU.IsLearned())
            {
                Show_POP(Message, Caption);
                return;
            }
            else
            {
                Message = "";
                Caption = "";
            }

            //현재 상태
            //string[] Data_List = Present_Data_Text.Text.ToString().Trim().Split(',');

        
            if (CPU.Get_Keys() != null && CPU.IsLearned())
            {
                string Data_Stream = CPU.Read_File(fath);

                //데이터 파싱
                string[] Data_Step = CPU.Input_Data(Data_Stream);

                List<string> Data_List = new List<string>();

                for (int i = 0; i < Data_Step.Length; i++)
                {
                    Data_List.Add(Data_Step[i]);
                }

                CPU.Learning(Data_List);

                /*
                if( CPU.Get_Window_Size() != Data_List.Length)
                {
                    Message = "관측범위와 데이터수가 다릅니다. 관측범위 = "
                        + CPU.Get_Window_Size() + "  입력한 데이터수 = " + Data_List.Length;

                    Caption = "오류발생";

                    Show_POP(Message, Caption);

                    return;

                }
                */
                /*
                for (int i = 0; i < Data_List.Length; i++)
                {
                    if (!CPU.Get_Keys().Contains(Data_List[i]))
                    {
                        Message = "등록되어 있지 않은 데이터가 있습니다.";
                        Caption = "오류발생";

                        Show_POP(Message, Caption);

                        return;
                    }
                }
                */

            }
            

            //double[] Result = CPU.Predict(Data_List);
            double[] Result = CPU.Predict(); //마르코프

            double Total = 0;

            //전부 0이하 인지 아닌지
            /*
            Console.WriteLine("사용자 입력 신호");
            for (int i = 0; i < Data_List.Length; i++)
            {
                Console.Write(Data_List[i] + "  ");
            }
            Console.WriteLine();
            Console.WriteLine();
            */

            double MAX = Result[0];
            int best = 0;
            Console.WriteLine("예측값");
            for(int i = 0; i < Result.Length; i++)
            {
                Console.Write(Result[i] + "  ");
                
                if(MAX < Result[i])
                {
                    MAX = Result[i];
                    best = i;
                }
                Total = Total + Result[i];
            }
            Console.WriteLine();
            Console.WriteLine();
            
            List<string> Keys = CPU.Get_Keys();

            //ResultGrid.Rows.Clear();
            ResultGrid.Rows.Add();
            for (int i = 0; i < Result.Length; i++)
            {
                Result[i] = (Result[i]) / Total;

                Message = Message +  Keys[i] + " = " + Math.Round(Result[i] * 100, 4) + "%  ";

                ResultGrid[Keys[i].Trim() + "_Columns", Result_Count].Value = Math.Round(Result[i] * 100, 4);
            }
            Message = Message + " 예측 데이터 > " + Keys[best];


            Caption = "예측결과 : " + Keys[best];

            Result_Count++;

            Show_POP(Message, Caption);
        }

        /*************************************************************************
         *        지원 함수
         * ********************************************************************/

        public string ShowFileOpenDialog()
        {
            //파일오픈창 생성 및 설정
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "학습 데이터 파일 불러오기";
            ofd.FileName = "";
            ofd.Filter = "모든 파일 (*.*) | *.*";

            //파일 오픈창 로드
            DialogResult dr = ofd.ShowDialog();

            //OK버튼 클릭시
            if (dr == DialogResult.OK)
            {
                //File명과 확장자를 가지고 온다.
                string fileName = ofd.SafeFileName;
                //File경로와 File명을 모두 가지고 온다.
                string fileFullName = ofd.FileName;
                //File경로만 가지고 온다.
                string filePath = fileFullName.Replace(fileName, "");

                //출력 예제용 로직
                /*
                label1.Text = "File Name  : " + fileName;
                label2.Text = "Full Name  : " + fileFullName;
                label3.Text = "File Path  : " + filePath;
                */
                //File경로 + 파일명 리턴
                return fileFullName;
            }
            //취소버튼 클릭시 또는 ESC키로 파일창을 종료 했을경우
            else if (dr == DialogResult.Cancel)
            {
                return "-1";
            }

            return "-1";
        }

        //오류창 출력
        public void Show_POP(string message, string caption)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void ResultGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //학습모델 저장하기
        private void Save_Model_Button_Click(object sender, EventArgs e)
        {
            string Message = "저장완료";
            string Caption = "알림";

            if (CPU.IsLearned())
            {
                CPU.Save_Model();
            }
            else
            {
                Message = "아직 학습이 되지 않았습니다.";
                Caption = "오류발생";
            }

            Show_POP(Message, Caption);
        }

        //학습모델 불러오기
        private void Load_Model_Button_Click(object sender, EventArgs e)
        {
            CPU.Load_Model();

            Grid_Register();

            Show_POP("불러오기 완료", "알림");
        }

        public void Grid_Register()
        {
            List<string> Keys = CPU.Get_Keys();

            ResultGrid.Columns.Clear();

            for (int i = 0; i < Keys.Count; i++)
            {
                ResultGrid.Columns.Add(Keys[i].Trim() + "_Columns", Keys[i].Trim());
            }
        }
    }


    
}
