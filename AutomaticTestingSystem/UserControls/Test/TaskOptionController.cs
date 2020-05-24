using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Controller;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.Test
{
    public class TaskOptionController : Controller
    {
        public TaskOptionController(PropertyChangedModel model) : base(model)
        {
            Start = new RelayCommand(StartCommand);
        }

        public RelayCommand Start { get; }

        public RelayCommand Calibration { get; }

        private void Initialize()
        {
            foreach (var top in SystemSettings.GroupItems)
            {
                foreach (var item in top.SubItems)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        item.MeasValue = "NA";
                        item.Result = "Waiting . . .";
                    });
                }
            }
        }
        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="obj"></param>
        private void StartCommand(object obj)
        {
            Task.Factory.StartNew(() =>
            {
                //初始化测试序列
                Initialize();
            }).ContinueWith(t =>
            {
                for (var i = 0; i < SystemSettings.GroupItems.Count; i++)
                {
                    var topItem = SystemSettings.GroupItems[i];
                    for (var j = 0; j < topItem.SubItems.Count; j++)
                    {
                        var item = topItem.SubItems[j];
                        var task = Task.Factory.StartNew(() =>
                        {
                            //运行指定函数
                            return this.MethodInvoke<object>(SystemSettings.Procedure, item.SelectedProcedure, item);
                        }).ContinueWith(tt =>
                        {
                            //UI线程中更新显示结果
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                item.MeasValue = tt.Result?.Data?.ToString();
                                var value = Convert.ToSingle(tt.Result?.Data);
                                var lowerPass = true;
                                var upperPass = true;
                                if (item.LowerOperator != OperatorEnum.None)
                                    lowerPass = IsInLimit(value, Convert.ToSingle( item.LowerLimit), item.LowerOperator);                                                           
                                
                                if (item.UpperOperator != OperatorEnum.None)
                                    upperPass = IsInLimit(value, Convert.ToSingle(item.UpperLimit), item.UpperOperator);

                                item.Result = lowerPass && upperPass ? "Pass" : "Fail";
                            });
                            if (item.AsVariable)
                            {
                                //存储当前结果为变量
                                if (SystemSettings.VariantDic.ContainsKey(item.VariantName))
                                    SystemSettings.VariantDic[item.VariantName] = tt.Result?.Data;
                                else
                                    SystemSettings.VariantDic.Add(item.VariantName, tt.Result?.Data);
                            }
                        });
                        if (!item.Independent)
                            task.Wait();
                        if (item.DelayMS > 0)
                            Thread.Sleep(item.DelayMS);
                    }
                }
            });

        }



        private bool IsInLimit(float f1, float f2, OperatorEnum op )
        {
            switch (op)
            {
                case OperatorEnum.Equal:
                    return f1 == f2;
                case OperatorEnum.LessThan:
                    return f1 < f2;
                case OperatorEnum.LessThanAndEqual:
                    return f1 <= f2;
                case OperatorEnum.MoreThan:
                    return f1 > f2;
                case OperatorEnum.MoreThanAndEqual:
                    return f1 >= f2;
                default:
                    return false;
            }




        }



    }
}
