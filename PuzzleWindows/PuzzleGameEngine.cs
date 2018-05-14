using System;

namespace PuzzleWindows
{
    internal class PuzzleGameEngine
    {
        int[] theViewIndices; //Indices == 인덱스의 복수형
        private int puzzleSize = 16;

        public PuzzleGameEngine()
        {
            //이미지 index 가져오기
            theViewIndices = new int[puzzleSize]; 

            for(int i = 0; i < theViewIndices.Length; i++)
            {
                theViewIndices[i] = i;

            }

            //이미지 index 섞기

            Random rnd = new Random(); //seed값을 주지 않아서 for문 안에 넣을 경우 매번 생성하게 되어 매번 똑같은 난수만 발생하게 되니 제대로된 작동 x
            for (int n = 0; n < 10000; n++)
            {
                
                int i = rnd.Next(0, theViewIndices.Length);
                //int j = rnd.Next(0, theViewIndices.Length);

                //Swap(i, j);
                Change(i); //랜덤하게 눌러서 빈칸이 주변에 있는칸을 옮기는 방식으로 함. 별로 많이 안바뀌므로 반복횟수를 늘려야함
             
            }

            

        }

        private void Swap(int i, int j)
        {
            int tmpSwap = theViewIndices[i];
            theViewIndices[i] = theViewIndices[j];
            theViewIndices[j] = tmpSwap;
        }

        internal int GetViewIndex(int index)
        {
            return theViewIndices[index];
        }

        internal void Change(int touchedIndex)
        {
            //터치한 인덱스 상하좌우에 빈 인덱스 있다면
            //교환
            if(GetEmptyindex()/4==touchedIndex/4 && //빈 카드랑 누른 카드가 같은 라인에 있는지
                (GetEmptyindex() == touchedIndex-1 || //왼쪽
                GetEmptyindex() == touchedIndex + 1) || //오른쪽
                GetEmptyindex() == touchedIndex - 4 || //위
                GetEmptyindex() == touchedIndex + 4) //아래
            {
                Swap(GetEmptyindex(), touchedIndex);
            }
        }

        private int GetEmptyindex()
        {
            for(int i = 0; i < puzzleSize; i++)
            {
                if (theViewIndices[i] == puzzleSize - 1) // 마지막 그림이라면
                {
                    return i;
                } 
            }
            return -1;
        }

        internal bool isEnd()
        {
            int count = 0;
            for(int i = 0; i < puzzleSize; i++)
            {
                if (theViewIndices[i] == i) count++;
            }

            return count == puzzleSize;
        }
    }
}