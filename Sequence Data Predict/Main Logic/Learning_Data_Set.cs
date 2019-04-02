using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequence_Data_Predict
{
    public class Learning_Data_Set
    {
        public List<List<double>> Signal_Data_Set; //최종 학습 데이터 집합
        public List<List<double>> Target_Data_Set; //최종 목표치 데이터 집합
        //public List<double>   Single_Target_Data_Set; //최종 목표치 데이터 집합
        public int Input_Length;
        public int Result_Length;
    }
}
