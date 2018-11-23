﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJAPlayer3
{
    /// <summary>
    /// 段位認定を管理するクラス。
    /// </summary>
    class Dan_C
    {
        public Dan_C()
        {
        }

        /// <summary>
        /// 段位認定の条件を初期化し、生成します。
        /// </summary>
        /// <param name="examType">条件の種別。</param>
        /// <param name="value">条件の合格量。</param>
        /// <param name="examRange">条件の合格の範囲。</param>
        public Dan_C(Exam.Type examType, int[] value, Exam.Range examRange)
        {
            IsEnable = true;
            NotReached = false;
            SetExamType(examType);
            SetValue(value[0], value[1]);
            SetExamRange(examRange);
        }

        /// <summary>
        /// 条件と現在の値を評価して、クリアしたかどうかを判断します。
        /// </summary>
        /// <param name="nowValue">その条件の現在の値。</param>
        public bool Update(int nowValue)
        {
            var isChangedAmount = false;
            if (!IsEnable) return isChangedAmount;
            if (Amount < nowValue) isChangedAmount = true;
            if (Range == Exam.Range.Less && nowValue > Value[0]) isChangedAmount = false; // n未満でその数を超えたらfalseを返す。
            Amount = nowValue;
            switch (Type)
            {
                case Exam.Type.Gauge:
                    SetCleared();
                    break;
                case Exam.Type.JudgePerfect:
                        SetCleared();
                    break;
                case Exam.Type.JudgeGood:
                        SetCleared();
                    break;
                case Exam.Type.JudgeBad:
                        SetCleared();
                    break;
                case Exam.Type.Score:
                        SetCleared();
                    break;
                case Exam.Type.Roll:
                        SetCleared();
                    break;
                case Exam.Type.Hit:
                        SetCleared();
                    break;
                case Exam.Type.Combo:
                        SetCleared();
                    break;
                default:
                    break;
            }
            return isChangedAmount;
        }

        /// <summary>
        /// 段位認定の条件が有効であるかどうかを返します。
        /// </summary>
        /// <returns>段位認定の条件が有効であるかどうか。</returns>
        public bool GetEnable()
        {
            return this.IsEnable;
        }

        /// <summary>
        /// 各合格条件のボーダーを設定します。
        /// </summary>
        /// <param name="redValue">赤合格条件</param>
        /// <param name="goldValue">金合格条件</param>
        public void SetValue(int redValue, int goldValue)
        {
            this.Value[0] = redValue;
            this.Value[1] = goldValue;
        }

        /// <summary>
        /// 各合格条件のボーダーを返します。
        /// </summary>
        /// <param name="isGoldValue">trueを指定すると、金合格条件を返します。</param>
        /// <returns>合格条件の値。</returns>
        public int GetValue(bool isGoldValue)
        {
            return isGoldValue == true ? this.Value[1] : this.Value[0];
        }

        /// <summary>
        /// 現在の値を設定します。
        /// </summary>
        /// <param name="amount">現在の値。</param>
        public void SetAmount(int amount)
        {
            this.Amount = amount;
        }

        /// <summary>
        /// 現在の値を返します。
        /// </summary>
        /// <returns>現在の値。</returns>
        public int GetAmount()
        {
            return this.Amount;
        }

        /// <summary>
        /// 条件の種別を返します。
        /// </summary>
        /// <returns>条件の種別</returns>
        public Exam.Type GetExamType()
        {
            return this.Type;
        }

        /// <summary>
        /// 条件の種別を設定します。
        /// </summary>
        /// <param name="type">条件の種別。</param>
        private void SetExamType(Exam.Type type)
        {
            this.Type = type;
        }

        /// <summary>
        /// 条件の範囲を返します。
        /// </summary>
        /// <returns>条件の範囲</returns>
        public Exam.Range GetExamRange()
        {
            return this.Range;
        }

        /// <summary>
        /// 条件の範囲を設定します。
        /// </summary>
        /// <param name="range"></param>
        private void SetExamRange(Exam.Range range)
        {
            this.Range = range;
        }

        /// <summary>
        /// 条件にクリアしているかどうか返します。
        /// </summary>
        /// <returns>条件にクリアしているかどうか。</returns>
        public bool[] GetCleared()
        {
            return IsCleared;
        }

        /// <summary>
        /// 条件と現在の値をチェックして、合格もしくは金合格をしてるか否かを更新する。
        /// </summary>
        private void SetCleared()
        {
            if (Range == Exam.Range.More)
            {
                if (Amount >= Value[0])
                {
                    IsCleared[0] = true;
                    if (Amount >= Value[1])
                    {
                        IsCleared[1] = true;
                    }
                }
                else
                {
                    IsCleared[0] = false;
                    IsCleared[1] = false;
                }
            }
            else
            {
                if (Amount < Value[1])
                {
                    IsCleared[1] = true;
                }
                else
                {
                    IsCleared[1] = false;
                }
                if (Amount < Value[0])
                {
                    IsCleared[0] = true;
                }
                else
                {
                    IsCleared[0] = false;
                }
            }        
        }
        
        /// <summary>
        /// ゲージの描画のための百分率を返す。
        /// </summary>
        /// <returns>Amountの百分率。</returns>
        public int GetAmountToPercent()
        {
            var percent = 0.0D;
            if(Value[0] == 0)
            {
                return 0;
            }
            if(Range == Exam.Range.More)
            {
                switch (Type)
                {
                    case Exam.Type.Gauge:
                    case Exam.Type.JudgePerfect:
                    case Exam.Type.JudgeGood:
                    case Exam.Type.JudgeBad:
                    case Exam.Type.Score:
                    case Exam.Type.Roll:
                    case Exam.Type.Hit:
                    case Exam.Type.Combo:
                        percent = 1.0 * Amount / Value[0];
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (Type)
                {
                    case Exam.Type.Gauge:
                    case Exam.Type.JudgePerfect:
                    case Exam.Type.JudgeGood:
                    case Exam.Type.JudgeBad:
                    case Exam.Type.Score:
                    case Exam.Type.Roll:
                    case Exam.Type.Hit:
                    case Exam.Type.Combo:
                        percent = (1.0 * (Value[0] - Amount)) / Value[0];
                        break;
                    default:
                        break;
                }
            }
            percent = percent * 100.0;
            if (percent < 0.0)
                percent = 0.0D;
            if (percent > 100.0)
                percent = 100.0D;
            return (int)percent;
        }

        /// <summary>
        /// 条件に達成できる見込みがあるかどうか値を代入します。
        /// </summary>
        /// <param name="notReached">到達したかどうか。</param>
        public void SetReached(bool notReached)
        {
            NotReached = notReached;
        }

        /// <summary>
        /// 条件に達成できる見込みがあるかどうかを返します。
        /// </summary>
        /// <returns>条件に達成できる見込みがあるかどうか。</returns>
        public bool GetReached()
        {
            return NotReached;
        }


        // オーバーライドメソッド
        /// <summary>
        /// ToString()のオーバーライドメソッド。段位認定モードの各条件の現在状況をString型で返します。
        /// </summary>
        /// <returns>段位認定モードの各条件の現在状況。</returns>
        public override string ToString()
        {
            return String.Format("Type: {0} / Value: {1}/{2} / Range: {3} / Amount: {4} / Clear: {5}/{6} / Percent: {7} / NotReached: {8}", this.Type, this.Value[0], this.Value[1], this.Range, this.Amount, this.IsCleared[0], this.IsCleared[1], this.GetAmountToPercent(), this.NotReached);
        }


        // フィールド
        /// <summary>
        /// その条件が有効であるかどうか。
        /// </summary>
        private bool IsEnable;
        /// <summary>
        /// 条件の種別。
        /// </summary>
        private Exam.Type Type;
        /// <summary>
        /// 条件の範囲。
        /// </summary>
        private Exam.Range Range;
        /// <summary>
        /// 条件の値。
        /// </summary>
        public int[] Value = new int[] { 0, 0 };
        /// <summary>
        /// 量。
        /// </summary>
        public int Amount;
        /// <summary>
        /// 条件をクリアしているか否か。
        /// </summary>
        public readonly bool[] IsCleared = new[] { false, false };

/// <summary>
        /// 条件の達成見込みがなくなったら、真になる。
        /// この変数が一度trueになれば、基本的にfalseに戻ることはない。
        /// (スコア加算については、この限りではない。)
        /// </summary>
        private bool NotReached = false;
    }

    static class Exam
    {
        /// <summary>
        /// 段位認定の条件の種別。
        /// </summary>
        public enum Type
        {
            Gauge,
            JudgePerfect,
            JudgeGood,
            JudgeBad,
            Score,
            Roll,
            Hit,
            Combo
        }

        /// <summary>
        /// 段位認定の合格範囲。
        /// </summary>
        public enum Range
        {
            /// <summary>
            /// 以上
            /// </summary>
            More,
            /// <summary>
            /// 未満
            /// </summary>
            Less
        }
    }
}
