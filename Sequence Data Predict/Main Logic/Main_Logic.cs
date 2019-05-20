/*
2019-03-20

본 프로그램 주요 기능 구현
GUI단에서 함수로 하나하나 사용하면 된다.

1. 학습 데이터 입력
2. 데이터 학습
3. 현재 데이터 입력
4. 예측결과 파일로 출력

5. 신경망 저장
6. 신경망 불러오기



작업내용
1. 파일 읽어오기
2. 시간에 따라 배치 크기만큼 쪼개기
3. 입력 데이터쌍을 생성
4. 각각 신호로 변환

작성자 서지민

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sequence_Data_Predict
{
    public class Main_Logic
    {
        private int Window_Size = 5; //관측범위
        private Neural_Network nn;
        private Markov markov;
        private File_Management file_api;

        string Data_Fath;
        string[] Last_Window_Data;
        //string Origin_Data;
        List<string> Keys;
        bool Learned = false;
        int past;

        public static Random r = new Random();

        public static double RandomRange(double min, double MAX)
        {
            return r.NextDouble() * (MAX - min) + min;
        }

        public void Set_pass(int past)
        {
            markov.Set_past(past);
            this.past = past;
            Console.WriteLine("MainLogic past " + past);
        }

        public void Set_Fath(string fath)
        {
            Data_Fath = fath;
        }

        public void Show_Weight()
        {
            nn.Show_Weight();
        }

        public int Get_Window_Size()
        {
            return this.Window_Size;
        }

        public List<string> Get_Keys()
        {
            return Keys;
        }

        public bool IsLearned()
        {
            return Learned;
        }

        public Main_Logic()
        {
            file_api = new File_Management();
            nn = new Neural_Network();
        }

        public void Set_Window(int w)
        {
            Window_Size = w;
        }


        /*************************************************************************
         *        주 함수
         * ********************************************************************/

        //파일 읽기
        public string Read_File(string fath)
        {
            string Read_Data = ""; //읽어온 데이터

            List<string> Receive= file_api.ReadLine(fath);

            Read_Data = Receive[0];

            Console.WriteLine("읽어온 데이터 원본 = " + Read_Data);

            return Read_Data;
        }

       //마르코프 학습하기
        public void Learning(List<string> data_list)
        {
            Keys_Function(data_list);
            markov.Keys = Keys;
            markov.Learning(data_list);

            Learned = true;
            markov.Set_past(past);
            file_api.Close_File_API();
        }

        //학습하기
        public void Learning(string [] Data, int Epoch, double MAX_Error, int Window)
        {
            this.Window_Size = Window;

            //데이터 분석
            Learning_Data_Set data_set = Data_Parser(Data); //데이터 분석기

            //데이터 꺼내기
            List<List<double>> Signal_Data_Set = data_set.Signal_Data_Set;
            List<List<double>> Target_Data_Set = data_set.Target_Data_Set;
            int Input_Length = data_set.Input_Length;
            int Result_Length = data_set.Result_Length;

            int Stage = 8;

            //신경망 구조 정의
            int[] Class_Size = new int[Stage];
            Class_Size[0] = Input_Length - Result_Length;
            Class_Size[1] = (int) ((Input_Length - Result_Length) / 1.1);
            Class_Size[2] = (int)((Input_Length - Result_Length) / 1.2);
            Class_Size[3] = (int)((Input_Length - Result_Length) / 1.4);
            Class_Size[4] = (int)((Input_Length - Result_Length) / 1.6);
            Class_Size[5] = (int)((Input_Length - Result_Length) / 1.8);
            Class_Size[6] = (int)((Input_Length - Result_Length) / 2);
            Class_Size[Class_Size.Length - 1] = Result_Length;

            string[] Active_Function_List = new string[Stage];
            

            for(int i = 0; i < Active_Function_List.Length; i++)
            {
                Active_Function_List[i] = "TanH";
            }

            //Active_Function_List[Active_Function_List.Length - 1] = "Sigmoid";

            //신경망 생성
            nn = new Neural_Network( Input_Length, Class_Size, Result_Length, Active_Function_List);

            //학습시작
            //int Epoch = 50000;
            //double MAX = 0.6625;
            //double MAX = 2.60;

            nn.Learning(Signal_Data_Set, Target_Data_Set, Epoch, MAX_Error);

            Learned = true;
        }

       

        //현재 값을 입력받아 데이터 예측하기
        public double [] Predict(string [] Data)
        {

            List<double> Signal_Data = String_To_Data(Keys.ToArray(), Data);

            double[] Result = nn.Predict(Signal_Data.ToArray());

            //Write_MAX(Result);

            return Result;
        }

        //현재 값을 입력받아 데이터 예측하기
        public double[] Predict()
        {
            markov.Set_past(past);
            /*
            Console.WriteLine("자동 입력 데이터열");
            for(int i =0; i < Last_Window_Data.Length;i ++)
            {
                Console.Write(Last_Window_Data[i] +"  ");
            }
            Console.WriteLine();
            Console.WriteLine();

            List<double> Signal_Data = String_To_Data(Keys.ToArray(), Last_Window_Data);
            
            markov.Set_past(past);
            double[] Result = nn.Predict(Signal_Data.ToArray());
            */
            double[] Result = markov.Transition_Predict();
            /*
            for(int i =0; i< Result.Length; i++)
            {
                Result[i] = RandomRange(0.0, 1.0);
            }
            */


            //Write_MAX(Result);

            return Result;
        }

        public void Write_Data(string data)
        {
            file_api.WriteFile(this.Data_Fath, true);
            file_api.Write(data, true);
            Read_File(this.Data_Fath);
        }

        public void Write_MAX(double []Result)
        {
            double MAX = Result[0];
            int Best = 0;

            for(int i = 0; i < Result.Length; i++)
            {
                if(MAX < Result[i])
                {
                    MAX = Result[i];
                    Best = i;
                }
            }
            
            string Write_Data = "," + Keys[Best];

            
            file_api.WriteFile(this.Data_Fath, true);
            file_api.Write(Write_Data, true);
            Read_File(this.Data_Fath);

            Console.WriteLine(this.Data_Fath + "  Data >" + Write_Data);

            markov.Add(Keys[Best]);
          
        }

        //모델 저장
        public void Save_Model()
        {
            /*
             * 1. 신경망 구조 저장
             * 2. 활성함수 저장
             * 3. 가중치 저장
             * 4. Keys 저장
             */

            nn.Save_Model();
            Save_Keys();
            Save_Window();
        }

        //모델 불러오기
        public void Load_Model()
        {
            /*
           * 1. 신경망 구조 불러오기
           * 2. 활성함수 불러오기
           * 3. 신경망 생성
           * 4. 가중치 불러오기
           * 5. Keys 불러오기
           */
            //nn = new Neural_Network();
            nn.Load_Model();
            Load_Keys();
            Load_Window();
            Learned = true;
         
        }

        /*************************************************************************
        *        Window 파일 입출력
        * ********************************************************************/
        //저장하기
        public void Save_Window()
        {
            file_api = new File_Management();

            file_api.WriteFile("./Registered Window.window");

             file_api.WriteLine(this.Window_Size.ToString());
             Console.WriteLine(this.Window_Size);
    
            file_api.Close_File_API();
        }

        //불러오기
        public void Load_Window()
        {
            file_api = new File_Management();

            file_api.ReadFile("./Registered Window.window");

            List<string> Data = file_api.ReadLine("./Registered Window.window");

            this.Window_Size = int.Parse(Data[0]);

            file_api.Close_File_API();
        }




        /*************************************************************************
         *        Keys 파일 입출력
         * ********************************************************************/

        //저장하기
        public void Save_Keys()
        {
            file_api = new File_Management();

            file_api.WriteFile("./Registered Keys.keys");

            for(int i = 0; i < this.Keys.Count; i++)
            {
                file_api.WriteLine(Keys[i]);
                Console.WriteLine(this.Keys[i]);
            }
            //file_api.WriteLine("가");

            file_api.Close_File_API();
        }

        //불러오기
        public void Load_Keys()
        {
            file_api = new File_Management();

            file_api.ReadFile("./Registered Keys.keys");

            Keys = file_api.ReadLine("./Registered Keys.keys");

            file_api.Close_File_API();
        }


        /*************************************************************************
         *        지원 함수
         * ********************************************************************/
        //CSV 해석기
        public string[] Input_Data(string Data)
        {
            string[] Token_Data = Data.Split(',');

            return Token_Data;
        }

        public void Keys_Function(List<string> Data)
        {
            //히스토그램 분석
            Dictionary<string, int> Data_Histogram = new Dictionary<string, int>(); //데이터 히스토그램

            List<string> Keys = new List<string>(); //데이터 종류 
            List<string> Data_Batch = new List<string>(); //배치 사이즈

            List<string[]> String_Data_Set = new List<string[]>(); //문자열 학습 데이터 집합
            List<string> String_Target_Data_Set = new List<string>(); // 문자열 목표치 데이터 집합

            List<List<double>> Signal_Data_Set = new List<List<double>>(); //최종 학습 데이터 집합
            List<List<double>> Target_Data_Set = new List<List<double>>(); //최종 목표치 데이터 집합

            int Total_Data_Kind = 0;
            int Normalize_Count = 1;

            //데이터 종류 분석
            for (int i = 0; i < Data.Count; i++, Normalize_Count++)
            {
                string temp = Data[i];
                int Count = 0;

                //이미 있는 데이터
                if (Data_Histogram.ContainsKey(temp))
                {
                    Count = 0;
                    Count = Data_Histogram[temp];
                    Count = Count + 1;

                    Data_Histogram[temp] = Count;
                }
                //새로운 데이터
                else
                {
                    Data_Histogram.Add(temp, 1);
                    Keys.Add(temp);
                    Total_Data_Kind++;
                }
            }

            //최종 학습 데이터 종류
            Console.WriteLine("데이터 종류 ");
            for (int i = 0; i < Keys.Count; i++)
            {
                Console.Write(Keys[i] + "  ");
            }
            Console.WriteLine();
            Console.WriteLine();


            this.Keys = Keys;

            markov = new Markov(Keys);
        }

        //데이터 쪼개기
        public Learning_Data_Set Data_Parser(string [] Data)
        {
            /*
             * 1. 데이터 분석 어떤 종류가 얼마나 있는지
             * 2. 신경망 생성하기
             * 3. 학습하기
             */
            Learning_Data_Set Result_Data = new Learning_Data_Set();

            //히스토그램 분석
            Dictionary<string, int> Data_Histogram = new Dictionary<string, int>(); //데이터 히스토그램

            List<string> Keys = new List<string>(); //데이터 종류 
            List<string> Data_Batch = new  List<string>(); //배치 사이즈

            List<string[]> String_Data_Set = new List<string[]>(); //문자열 학습 데이터 집합
            List<string> String_Target_Data_Set = new List<string>(); // 문자열 목표치 데이터 집합

            List< List< double>> Signal_Data_Set = new List<List<double>>(); //최종 학습 데이터 집합
            List<List<double>> Target_Data_Set = new List<List<double>>(); //최종 목표치 데이터 집합

            int Total_Data_Kind = 0;
            int Normalize_Count = 1;

            //데이터 종류 분석
            for(int i = 0; i < Data.Length; i++, Normalize_Count++)
            {
                string temp = Data[i];
                int Count = 0;

                //이미 있는 데이터
                if( Data_Histogram.ContainsKey(temp))
                {
                    Count = 0;
                    Count = Data_Histogram[temp];
                    Count = Count + 1;

                    Data_Histogram[temp] = Count;
                }
                //새로운 데이터
                else
                {
                    Data_Histogram.Add(temp, 1);
                    Keys.Add(temp);
                    Total_Data_Kind++;
                }

                //데이터 표준화 
                //과측범위만큼 분리하기 - 표본화
                
                if( (i >= Window_Size))
                {

                    //거꾸로 들어가니 주의

                    //입력값 자르기
                    for(int back = i - 1; back > (i - 1)- Window_Size; back--)
                    {
                        Data_Batch.Add( Data[back]);
                    }

                    //다시 거꾸로 하면 원래상태로 온다.
                    Data_Batch.Reverse();

                    // 문자열 학습 데이터 집합 생성
                    String_Data_Set.Add(  Data_Batch.ToArray() );

                    Console.WriteLine("Batch Data");
                    Console.Write("과거 상태 > ");
                    for (int t = 0; t < Data_Batch.ToArray().Length; t++)
                    {
                        Console.Write(Data_Batch.ToArray()[t] + "  ");
                    }
                    Console.WriteLine("         다음 상태 > " + Data[i]);
                    Console.WriteLine();
                    Console.WriteLine();

                    //목표치 자르기
                    String_Target_Data_Set.Add(Data[i]);

                    Data_Batch.Clear();
                    Normalize_Count = 0;
                }
                
              
            }

            //최종 학습 데이터 종류
            Console.WriteLine("데이터 종류 ");
            for (int i = 0; i < Keys.Count; i++)
            {
                Console.Write( Keys[i] + "  ");
            }
            Console.WriteLine();
            Console.WriteLine();


            this.Keys = Keys;

            
            
            //최종 학습 데이터 생성하기
            for ( int i = 0; i < String_Data_Set.Count; i++)
            {
                //하나하나 신호로 변환 - 양자화

                //입력값
                List<double> Signal_Data = String_To_Data( Keys.ToArray() , String_Data_Set[i]);

                Signal_Data_Set.Add(Signal_Data);

                //목표값
                List<double> Target_Data = String_To_Data(Keys.ToArray(), String_Target_Data_Set[i]);

                Target_Data_Set.Add(Target_Data);
            }
            

            //신경망 입력길이 계산
            int Input_Length = Total_Data_Kind * Window_Size;

            //신경망 출력길이 계산
            int Result_Length = Total_Data_Kind;


            //최종 데이터 정보 생성
            Result_Data.Signal_Data_Set = Signal_Data_Set;
            Result_Data.Target_Data_Set = Target_Data_Set;
            Result_Data.Input_Length = Input_Length;
            Result_Data.Result_Length = Result_Length;

            //this.Last_Window_Data = String_Data_Set[String_Data_Set.Count - 1];

            Console.WriteLine("마지막 범위 데이터 " + Data.Length);

            this.Last_Window_Data = new string[Window_Size];

            for(int i = (Data.Length) - Window_Size, j = 0; i < Data.Length; i++, j++)
            {
                this.Last_Window_Data[j] =  Data[i];
            }

            for(int i = 0; i < this.Last_Window_Data.Length; i++)
            {
                Console.Write(this.Last_Window_Data[i] + "  ");
            }
            Console.WriteLine();
            Console.WriteLine();
            
            return Result_Data;
        }

         // 배치 사이즈 하나 따와서 신호로 변환하기
        public List<double> String_To_Data(string [] Kind , string [] Data)
        {
            List<double> Result_Array = new List<double>();

            double temp = 0;
 
            for(int i = 0; i < Data.Length; i++)
            {
                for(int j = 0; j < Kind.Length; j++)
                {
                    //해당 종류의 값이 오면
                    if( Data[i] == Kind[j])
                    {
                        temp = 1; //1로 변환한다.
                        //break;
                    }
                    else
                    {
                        temp = -1;
                    }
                  
                    Result_Array.Add(temp);
                }

            }
         
            return Result_Array;
        }


        // 배치 사이즈 하나 따와서 신호로 변환하기 오버로딩
        public List<double> String_To_Data(string[] Kind, string Data)
        {
            List<double> Result = new List<double>();

            double temp = 0;

            Console.WriteLine("Target Array");
            for (int j = 0; j < Kind.Length; j++)
            {
                    //해당 종류의 값이 오면
                    if (Data == Kind[j])
                    {
                        temp = 1; //1로 변환한다.
                        //break;
                    }
                    else
                    {
                        temp = -1;
                    }

                Console.Write( temp +"  ");
                Result.Add(temp);
            }
            Console.WriteLine();
            Console.WriteLine();

            return Result;
        }
    }
}
