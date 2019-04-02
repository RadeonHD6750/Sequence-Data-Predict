/*
2019-03-20

1. 인공신경망 라이브러리 결합
2. 시계열 데이터 예측 동작 정의

작성자 서지민

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sequence_Data_Predict
{
    public class Neural_Network
    {
        private Jimin_ANN fnn;
        private File_Management file_api;

        int Data_Length;
        int[] Class_Size;
        string [] Active_Function_Name_List;

        //생성자
        public Neural_Network()
        {
            file_api = new File_Management();
        }

        //생성자 오버로딩
        public Neural_Network(int Data_Length, int[] Class_Size, int Result_Length, string[] Active_Function_List)
        {
            AI_Build(Data_Length, Class_Size, Result_Length, Active_Function_List);
        }

        //신경망 생성하기
        public void AI_Build(int Data_Length, int[] Class_Size, int Result_Length, string[] Active_Function_List)
        {
            Console.WriteLine("파일을 읽어와 신경망 구축 시작 \n\n");
            this.Data_Length = Data_Length;
            this.Class_Size = Class_Size;
            this.Active_Function_Name_List = Active_Function_List;

            Class_Size[0] = Data_Length;

            fnn = new Jimin_ANN(Data_Length, Class_Size, Result_Length, Active_Function_List);
            Console.WriteLine("파일을 읽어와 신경망 구축 종료\n\n");
        }

        //학습하기
        public void Learning(List< List<double>> Signal_Set, List< List<double>> Target_Set, int Epoch, double Max_Error)
        {
            Console.WriteLine("신경망 학습시작 \n\n\n");

            //하이퍼파라미터
            double a = 0.001;
            double b = 0.0005;
  
            fnn.Set_Learning(a, b);

            double Total_Cost = 0;
            double Was_Cost = 99999;

            List<double[]> Result_Set = new List<double[]>();

            for(int Epoch_Time = 0; Epoch_Time < Epoch; Epoch_Time++)
            {
                Total_Cost = 0;

                for(int Batch = 0; Batch < Signal_Set.Count; Batch++)
                {
                    //최초 출력
                    double[] Result = fnn.Propagate(Signal_Set[Batch].ToArray() );

                    Result_Set.Add(Result);

                    //오차신호 계산
                    double[] Error_Array = fnn.Error_Function( Target_Set[Batch].ToArray() , Result);

                    //신경망 오차 계산
                    double Cost = fnn.Cost_Function(Target_Set[Batch].ToArray(), Result);

                    Total_Cost = Total_Cost + Cost;

                    fnn.Update( Error_Array, Signal_Set[Batch].ToArray());
                }

                Total_Cost = Total_Cost / Signal_Set.Count;

                if(Epoch_Time % 200 == 0)
                {
                    Console.WriteLine("Epoch " + Epoch_Time + "   Cost " + Total_Cost  +  "  진행률 " + (  Math.Round((Max_Error) / (Total_Cost) * 100,2) )+ "%");
                }

                if ((Total_Cost < Max_Error) || (Was_Cost + a < Total_Cost))
                {
                    Console.WriteLine("Final Epoch " + Epoch_Time + "   Cost " + Total_Cost);
                    break;
                }

                Was_Cost = Total_Cost;
            }
            Console.WriteLine("Final Epoch   Cost " + Total_Cost);
            //Console.WriteLine("신경망 모델 정확도   " + Accu(Target_Set, Result_Set) );
        }

        //정확도
        public double Accu(List<List<double>> Target_Set, List<double[]> Result_Set)
        {
            double Accu = 0;
            double Count = 0;

            for(int i = 0; i < Target_Set.Count; i++)
            {
                double[] Result = Max_Function(Result_Set[i]);

                Count = 0;
                for (int j = 0; j < Result.Length; j++)
                {
                    if( Target_Set[i][j] == Result[j])
                    {
                        Count++;
                    }
                }

                //패턴 일치
                if(Count == Result.Length)
                {
                    Accu++;
                }

            }

            Accu = Math.Round((Accu / Target_Set.Count) * 100, 2);

            return Accu;
        }

        public double[] Max_Function(double[] Result)
        {
            double[] Max_List = Result;

            double max = Result[0];
            int best = 0;

            for(int i = 0; i < Result.Length; i++)
            {
                if(max < Result[i])
                {
                    max = Result[i];
                    best = i;
                }

                Max_List[i] = -1;
            }

            Max_List[ best] =  1;

            return Max_List;
        }

        //예측하기
        public double [] Predict(double [] Data)
        {
            double[] Result_String = fnn.Propagate(Data);

            return Result_String;
        }

        /****************************************************************************************************************
         *  각종 파일 입출력 정의
         * **************************************************************************************************************/

        //학습 모델 저장
        public void Save_Model()
        {
            file_api = new File_Management();
            Constructe_Commit(Data_Length, Class_Size); //신경망 구조 저장
            Active_Function_Constructe_Commit(Active_Function_Name_List); //활성함수 저장
            Weight_Commit(Class_Size); //가중치 저장
        }

        //학습 모델 불러오기 
        public void Load_Model()
        {
            file_api = new File_Management();
            File_Read_Constructe();

            File_Read_Weight_Load(Class_Size);
        }

        public void Show_Weight()
        {
            fnn.Show_Weight();
        }

        //신경망 구조 정의 파일 저장
        public void Constructe_Commit(int Data_Length,int [] Class_Size)
        {
            this.file_api.WriteFile("./Neural Network Constructed.construct");

            file_api.WriteLine(Data_Length.ToString());
            file_api.WriteLine("0");
            file_api.WriteLine("0");
            file_api.WriteLine("0");
            file_api.WriteLine("0");

            for (int i = 0; i < Class_Size.Length; i++)
            {
                if (i == Class_Size.Length - 1)
                {
                    file_api.WriteLine(Class_Size[i].ToString());
                }
                else
                {
                    file_api.Write(Class_Size[i].ToString() + ",");
                }
            }
            file_api.WriteLine(Class_Size[ Class_Size.Length -1  ].ToString());

            file_api.Close_File_API();
        }

        //신경망 활성함수 정의 파일 저장
        public void Active_Function_Constructe_Commit(string [] Active_Function_Name_List)
        {
            this.file_api.WriteFile("./Neural Network Active_Function_Constructed.construct");

            for (int i = 0; i < Active_Function_Name_List.Length; i++)
            {
                if (i == Active_Function_Name_List.Length - 1)
                {
                    file_api.WriteLine(Active_Function_Name_List[i].ToString());
                }
                else
                {
                    file_api.Write(Active_Function_Name_List[i].ToString() + ",");
                }
            }
            file_api.Close_File_API();
        }

        //가중치 저장
        public void Weight_Commit(int [] Class_Size)
        {
            file_api.WriteFile("./Policy Neural Network Weight Memory.weight");

            double Weight_Data;

          
                    for (int i = 1; i < Class_Size.Length; i++)
                    {

                        for (int j = 0; j < Class_Size[i]; j++)
                        {

                            for (int k = 0; k < Class_Size[i - 1] + 1; k++) //마지막 하나가 안써짐 +1
                            {
                                Weight_Data = fnn.Get_Weight(i - 1, j, k);
                                if (k >= Class_Size[i - 1])
                                {
                                    //file_api.Write(Math.Round(Weight_Data, 15) + "\n");
                                    file_api.Write(Weight_Data + "\n");
                                }
                                else
                                {
                                    //file_api.Write(Math.Round(Weight_Data, 15) + " ");
                                    file_api.Write(Weight_Data + " ");
                                }

                            }
                        }
                        file_api.Write("\n\n");
                    }

            file_api.Close_File_API();
        }

        //가중치 읽기
        public void File_Read_Weight_Load(int [] Class_Size)
        {
            file_api.ReadFile("./Policy Neural Network Weight Memory.weight");

            int count = 0;
            double[] Weight_Data;

            List<double[]> Weight_Data_List = file_api.Read_NN_Weight();

            //Console.WriteLine("가중치 불러오기 루틴 시작 \n\n");
            for (int i = 1; i < Class_Size.Length; i++)
            {

                for (int j = 0; j < Class_Size[i]; j++)
                {

                    Weight_Data = Weight_Data_List[count];
                    
                    for (int k = 0; k < Class_Size[i - 1] + 1; k++) //마지막 하나가 누락되어 읽어진다. + 1
                    {
                        fnn.Set_Weight(i - 1, j, k, Weight_Data[k]);

                        //Console.Write(Weight_Data[k] + "  ");
                    }
                    count++;
                    //Console.WriteLine();
                }
                //Console.WriteLine();
            }
            //Console.WriteLine();
            file_api.Close_File_API();
        }

        //구조를 읽어서 생성
        public void File_Read_Constructe()
        {
            int Width, Height, Window_Size, Stage, Kernal_Number;
            int [] Class_Size = null;
            List<string[]> Active_Function_List;
            int Result_Length;

            this.file_api.ReadFile("./Neural Network Constructed.construct");

            file_api.Read_NN_Constructe(out Width, out Height, out Window_Size, out Stage, out Kernal_Number, ref Class_Size, out Result_Length);

            this.file_api.ReadFile("./Neural Network Active_Function_Constructed.construct");
            Active_Function_List = file_api.Read_NN_Active_Function();

         
            AI_Build(Width, Class_Size, Result_Length, Active_Function_List[0]);

            this.Class_Size = Class_Size;
            this.Data_Length = Width;

            file_api.Close_File_API();
        }
    }
}
