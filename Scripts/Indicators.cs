using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    public Image healthBar, foodBar, waterBar;
        public float healthAmount = 100;
        public float foodAmount = 100;
        public float uiFoodAmount = 100;
        public float uiWaterAmount = 100;
        public float waterAmount = 100;
        
        private float changeFactor = 3f;

        public float secondsToEmptyFood = 150f;
        public float secondsToEmptyWater = 130f;
        public float secondsToEmptyHealth = 90f;
        

        // Start is called before the first frame update
        void Start()
        {
            healthBar.fillAmount = healthAmount / 100;
            foodBar.fillAmount = foodAmount / 100;
            waterBar.fillAmount = waterAmount / 100;
        }
        
        // Update is called once per frame
        void Update()
        {
            ChangeCharacteristics();
            if (foodAmount > 0)
            {
                foodAmount -= 100 / secondsToEmptyFood * Time.deltaTime;
                uiFoodAmount = Mathf.Lerp(uiFoodAmount, foodAmount, Time.deltaTime * changeFactor);
                foodBar.fillAmount = uiFoodAmount / 100;
            }
            if (waterAmount > 0)
            {
                waterAmount -= 100 / secondsToEmptyWater * Time.deltaTime;
                uiWaterAmount = Mathf.Lerp(uiWaterAmount, waterAmount, Time.deltaTime * changeFactor);
                waterBar.fillAmount = waterAmount / 100;
            }
     
            if(foodAmount <= 0)
            {
                healthAmount -= 100 / secondsToEmptyHealth * Time.deltaTime;
            }
            if(waterAmount <= 0)
            {
                healthAmount -= 100 / secondsToEmptyHealth * Time.deltaTime;
            }
            healthBar.fillAmount = healthAmount / 100;
        }
        public void ChangeFoodAmount(float changeValue)
        {
            foodAmount += changeValue;
        }
        public void ChangeWaterAmount(float changeValue)
        {
            waterAmount += changeValue;
        }
        public void ChangeHealthAmount(float changeValue)
        {
            healthAmount += changeValue;
        }
    
        public void ChangeCharacteristics()
        {
           
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                ChangeFoodAmount(-15);
                ChangeWaterAmount(-15);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                ChangeFoodAmount(10);
                ChangeWaterAmount(10);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                ChangeFoodAmount(60);
                ChangeWaterAmount(60);
                ChangeHealthAmount(60);
            }
        }
}
