using UnityEngine;


namespace Assets.Scripts.Logic.UI
{
    public class TabTool : SingletonUI<TabTool>
    {

        public GameObject[] tab_windows;

        void Start()
        {
            for (int i = 0; i < tab_windows.Length; i++)
            {
                if (i == 0)
                {
                    tab_windows[i].SetActive(true);
                }
                else
                {
                    tab_windows[i].SetActive(false);
                }
            }
            UIEventListener.Get(gameObject).onClick += OnSwitch;
        }

        public void OnSwitch(GameObject obj)
        {
            for (int i = 0; i < tab_windows.Length; i++)
            {
                string tab_window_name = tab_windows[i].name.ToLower();
                if (gameObject.name.Contains(tab_window_name))
                {
                    tab_windows[i].SetActive(true);
                    continue;
                }
                else
                { 
                    tab_windows[i].SetActive(false);
                }
            }

        }
    }
}
