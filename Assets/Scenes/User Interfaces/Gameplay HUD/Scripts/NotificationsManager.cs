using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas
{
    public class NotificationsManager : MonoBehaviour
    {
        [SerializeField] private GameObject notificationPanel;

        public void ShowNotification(Notification notification)
        {
            if(!notificationPanel) return;

            NotificationPanel panel = Instantiate(notificationPanel,transform).GetComponent<NotificationPanel>();
            panel.UpdatePanel(notification);

        }
    }
}
