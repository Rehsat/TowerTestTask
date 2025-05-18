using UniRx;
using UnityEngine.UI;

namespace Utils.Extensions
{
    public static class LayoutGroupExtension
    {
        //Вкл/выкл группы, чтобы она отрабатывала и после не потребляла лишних ресурсов
        public static void UpdateGroup(this LayoutGroup layoutGroup)
        {
            layoutGroup.enabled = true;
            Observable.TimerFrame(1).Subscribe((l =>
            {
                layoutGroup.enabled = false;
            }));
        }
    }
}
