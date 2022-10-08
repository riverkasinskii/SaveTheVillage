using UnityEngine;
using UnityEngine.UI;
public class SaveTheVillage : MonoBehaviour
{
    [SerializeField] private Text _wheatCount; // Пшеница
    [SerializeField] private Text _warriorCount; // Воины
    [SerializeField] private Text _peasantCount; // Крестьяне
    [SerializeField] private Text _gameMessages; // Игровые сообщения по событию
    [SerializeField] private Text _nextWave; // Поле с текстом, "Следующий набег: ";
    [SerializeField] private Text _enemiesAppear; // Поле с текстом, "Враги через: ";
    [SerializeField] private Text _nextWaveCounter; // Поле, показывающее следующую волну
    [SerializeField] private Text _enemiesAppearCounter; // Поле, через сколько появятся враги
    [SerializeField] private Text _experiencedAttacksCounter; // Поле для вывода кол-ва пережитых атак
    [SerializeField] private Text _producedGrainCounter; // Поле для вывода кол-ва собранной пшеницы
    [SerializeField] private Text _enemiesKilledCounter; // Поле для вывода кол-ва убитых врагов
    [SerializeField] private ImageTimer _harvestTimer; // Таймер урожая
    [SerializeField] private ImageTimer _eatingTimer; // Таймер еды
    [SerializeField] private ImageTimer _raidTimer; // Таймер нападения врагов
    [SerializeField] private Image _peasantTimer; // прогресс-бар создания крестьянина
    [SerializeField] private Image _warriorTimer; // Прогресс-бар создания воина
    [SerializeField] private Button hirePeasantButton; // Кнопка создания крестьянина
    [SerializeField] private Button hireWarriorButton; // Кнопка создания воина
    [SerializeField] private GameObject _victoryGame; // Панель победы
    [SerializeField] private GameObject _defeatGame; // Панель поражения
    [SerializeField] private GameObject _aboutGame; // Панель об игре
    [SerializeField] private GameObject _homeScreenGame; // Начальная панель игры
    [SerializeField] private GameObject _pauseGamePanel; // Панель паузы  
    [SerializeField] private Audio _soundGame; // Звук игры
    [SerializeField] private Audio _playSoundFood; // Звук еды
    [SerializeField] private Audio _playSoundEnemiesRaid; // Звук нападения врагов
    [SerializeField] private Audio _playSoundHarvesting; // Звук сбора урожая
    [SerializeField] private Audio _playSoundHireWarrior; // Звук создания воина
    [SerializeField] private Audio _playSoundHirePeasant; // Звук создания крестьянина
    private const int PEASANTS_FOR_VICTORY = 50; // Кол-во крестьян для победы
    private const int WHEAT_FOR_VICTORY = 1000; // Кол-во пшеницы для победы
    private const int WHEAT_PER_WARRIOR = 1; // Кол-во пшеницы нужное для прокорма воина
    private const int WHEAT_PER_PEASANT = 1; // Кол-во пшеницы которое может добыть один крестьянин
    private const int PEASANT_COST = 4; // Стоимость найма крестьян в ед. пшеницы
    private const int WARRIOR_COST = 2; // Стоимость найма воинов в ед. пшеницы
    private const float PEASANT_UPDATE_TIME = 4; // Переменная перезарядки найма крестьян в сек.
    private const float WARRIOR_UPDATE_TIME = 2; // Переменная перезарядки найма воинов в сек.
    private int _tempWheatCount; // Промежуточная переменная для подсчета пшеницы
    private int _tempWarriorCount; // Промежуточная переменная для подсчета воинов
    private int _tempPeasantCount; // Промежуточная переменная для подсчета крестьян   
    private int _countNextRaid; //  Подсчет волн врагов        
    private int _tempCounterAttacks; // Подсчет количества набегов
    private int _tempCounterProducedGrain; // Подсчет произведенного зерна
    private int _tempCounterKilledEnemies; // Подсчет убитых врагов
    private int _reverseCounter; // Переменная обратного отсчета
    private float _tempPeasantValue = -2; // Переменная для условия обновления крестьян
    private float _tempWarriorValue = -2; // Переменная для условия обновления воинов 
    private bool _isPausedGame; // Состояние паузы
    private bool _isSoundGame = true; // Состояние звука в игре

    // Start is called before the first frame update
    private void Start()
    {
        _homeScreenGame.SetActive(true);
        _aboutGame.SetActive(false);  
        _pauseGamePanel.SetActive(false);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateResources();
        UpdatePeasant();
        UpdateWarrior();
        EnemyRaid();
    }

    /// <summary>
    /// Кнопка рестарт(сброс всех нужных состояний)
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1;
        _victoryGame.SetActive(false);
        _defeatGame.SetActive(false);
        _raidTimer.CurrentTime = _raidTimer.MaxTime;
        _eatingTimer.CurrentTime = _eatingTimer.MaxTime;
        _harvestTimer.CurrentTime = _harvestTimer.MaxTime;
        _raidTimer.OnOffTimer = true;
        _eatingTimer.OnOffTimer = true;
        _harvestTimer.OnOffTimer = true;
        _tempWarriorCount = 0;
        _warriorCount.text = _tempWarriorCount.ToString();
        _tempPeasantCount = 0;
        _peasantCount.text = _tempPeasantCount.ToString();
        _tempWheatCount = 10;
        _wheatCount.text = _tempWheatCount.ToString();
        _gameMessages.text = string.Empty;
        _enemiesAppear.text = "Враги через: ";
        _countNextRaid = 0;
        _nextWaveCounter.text = string.Empty;
        _reverseCounter = 3;
        _enemiesAppearCounter.text = _reverseCounter.ToString();
        _nextWave.text = string.Empty;
        _tempCounterAttacks = 0;
        _tempCounterKilledEnemies = 0;
        _tempCounterProducedGrain = 0;
        _soundGame.Sound.Play();
    }

    /// <summary>
    /// Кнопка создания крестьянина
    /// </summary>
    public void CreatePeasant()
    {
        if (_tempWheatCount >= PEASANT_COST)
        {            
            _tempWheatCount -= PEASANT_COST;
            _wheatCount.text = _tempWheatCount.ToString();
            _tempPeasantValue = PEASANT_UPDATE_TIME;
            _playSoundHirePeasant.Sound.Play();
            hirePeasantButton.interactable = false;
        }               
    }
    /// <summary>
    /// Кнопка создания воина
    /// </summary>
    public void CreateWarrior()
    {
        if (_tempWheatCount >= WARRIOR_COST)
        {            
            _tempWheatCount -= WARRIOR_COST;
            _wheatCount.text = _tempWheatCount.ToString();
            _tempWarriorValue = WARRIOR_UPDATE_TIME;
            _playSoundHireWarrior.Sound.Play();
            hireWarriorButton.interactable = false;
        }                               
    }
    /// <summary>
    /// Кнопка вызова панели об игре
    /// </summary>
    public void AboutGame()
    {
        _aboutGame.SetActive(true);
    }

    /// <summary>
    /// Кнопка закрытия панели об игре
    /// </summary>
    public void ClosePanelAboutGame()
    {
        _aboutGame.SetActive(false);
    }

    /// <summary>
    /// Кнопка старт игры
    /// </summary>
    public void StartGame()
    {
        Restart();
        _homeScreenGame.SetActive(false);        
    }

    /// <summary>
    /// Продолжение игры
    /// </summary>
    public void PlayGame()
    {
        if (_isPausedGame == true)
        {      
            _pauseGamePanel.SetActive(false);
            Time.timeScale = 1;
            _soundGame.Sound.Play();            
        }
        _isPausedGame = false;
    }

    /// <summary>
    /// Пауза игры
    /// </summary>
    public void PauseGame()
    {
        if (_isPausedGame == false)
        {
            _pauseGamePanel.SetActive(true);
            Time.timeScale = 0;
            _soundGame.Sound.Pause();
        }
        _isPausedGame = true;
    }

    /// <summary>
    /// Кнопка выключения звука
    /// </summary>
    public void SoundGameOnOff()
    {
        if (_isSoundGame == true)
        {
            _soundGame.Sound.mute = true;
            _playSoundFood.Sound.mute = true;
            _playSoundEnemiesRaid.Sound.mute = true;
            _playSoundHarvesting.Sound.mute = true;
            _playSoundHirePeasant.Sound.mute = true;
            _playSoundHireWarrior.Sound.mute = true;
        }
        else
        {
            _soundGame.Sound.mute = false;
            _playSoundFood.Sound.mute = false;
            _playSoundEnemiesRaid.Sound.mute = false;
            _playSoundHarvesting.Sound.mute = false;
            _playSoundHirePeasant.Sound.mute = false;
            _playSoundHireWarrior.Sound.mute = false;
        }
        _isSoundGame =! _isSoundGame;
    }

    /// <summary>
    /// Метод обновления ресурсов
    /// </summary>
    private void UpdateResources()
    {
        if (_tempWheatCount < WARRIOR_COST)
        {
            hireWarriorButton.interactable = false;
        }
        if (_tempWheatCount < PEASANT_COST)
        {
            hirePeasantButton.interactable = false;
        }

        if (_harvestTimer.Tick == true)
        {            
            _tempWheatCount += _tempPeasantCount * WHEAT_PER_PEASANT;
            _tempCounterProducedGrain += _tempPeasantCount * WHEAT_PER_PEASANT;
            _wheatCount.text = _tempWheatCount.ToString();
            _playSoundHarvesting.Sound.Play();            

            if (_tempWheatCount >= WHEAT_FOR_VICTORY)
            {
                VictoryGame();
            }
        }
        if (_eatingTimer.Tick == true)
        {
            int checkRecourcesEating; // Проверка нехватки пшеницы для прокорма воина
            checkRecourcesEating = _tempWheatCount;
            _tempWheatCount -= _tempWarriorCount * WHEAT_PER_WARRIOR;

            if (_tempWheatCount >= 0)
            {
                _wheatCount.text = _tempWheatCount.ToString();
                _playSoundFood.Sound.Play();
            }
            else
            {
                _wheatCount.text = checkRecourcesEating.ToString();
                _gameMessages.text = "Недостаточно пшеницы для прокорма армии. " +
                    "Армия уменьшается на 1 воина";
                _tempWarriorCount -= 1;
                _warriorCount.text = _tempWarriorCount.ToString();
            }
                                  
        }        
    }

    /// <summary>
    /// Метод обновления крестьян
    /// </summary>
    private void UpdatePeasant()
    {               
        if (_tempPeasantValue > 0)
        {
            _tempPeasantValue -= Time.deltaTime;
            _peasantTimer.fillAmount = _tempPeasantValue / PEASANT_UPDATE_TIME;
        }
        else if (_tempPeasantValue > -1)
        {
            _peasantTimer.fillAmount = 1;
            hirePeasantButton.interactable = true;
            _tempPeasantCount += 1;
            _peasantCount.text = _tempPeasantCount.ToString();
            _tempPeasantValue = -2;
            if (_tempPeasantCount >= PEASANTS_FOR_VICTORY)
            {
                VictoryGame();
            }
        }
        else if (_tempWheatCount >= PEASANT_COST)
        {
            hirePeasantButton.interactable = true;
        }
    }

    /// <summary>
    /// Метод обновления воинов
    /// </summary>
    private void UpdateWarrior()
    {        
        if (_tempWarriorValue > 0)
        {
            _tempWarriorValue -= Time.deltaTime;
            _warriorTimer.fillAmount = _tempWarriorValue / WARRIOR_UPDATE_TIME;
        }
        else if (_tempWarriorValue > -1)
        {
            _warriorTimer.fillAmount = 1;
            hireWarriorButton.interactable = true;
            _tempWarriorCount += 1;
            _warriorCount.text = _tempWarriorCount.ToString();
            _tempWarriorValue = -2;
        }
        else if (_tempWheatCount >= WARRIOR_COST)
        {
            hireWarriorButton.interactable = true;
        }
    }

    /// <summary>
    /// Метод нападения врагов на деревню
    /// </summary>
    private void EnemyRaid()
    {
        if (_raidTimer.Tick == true)
        {            
            _gameMessages.text = string.Empty;
            _reverseCounter--;
            while (_reverseCounter >= 0)
            {
                _enemiesAppearCounter.text = _reverseCounter.ToString();
                if (_reverseCounter == 0)
                {
                    _gameMessages.text = "Враги напали!";
                    _enemiesAppear.text = string.Empty;
                    _enemiesAppearCounter.text = string.Empty;
                    _nextWave.text = "Следующий набег: ";
                    _nextWaveCounter.text = _countNextRaid.ToString();
                    _playSoundEnemiesRaid.Sound.Play();
                }
                break;
            }
            while (_reverseCounter <= 0)
            {
                _tempWarriorCount -= _countNextRaid;                
                _warriorCount.text = _tempWarriorCount.ToString();
                _playSoundEnemiesRaid.Sound.Play();

                if (_countNextRaid != 0)
                {
                    _gameMessages.text = $"В этой волне убито единиц армии: {_countNextRaid}";                    
                }    
                
                _countNextRaid++;                
                _nextWaveCounter.text = _countNextRaid.ToString();

                if (_tempWarriorCount > 0)
                {
                    _tempCounterKilledEnemies += _countNextRaid;
                    _tempCounterAttacks++;
                }
                else if(_tempWarriorCount < 0)
                {
                    _enemiesKilledCounter.text = _tempCounterKilledEnemies.ToString();
                    _producedGrainCounter.text = _tempCounterProducedGrain.ToString();
                    _experiencedAttacksCounter.text = _tempCounterAttacks.ToString();
                    DefeatGame();
                }
                break;
            }           
        }        
    }  

    /// <summary>
    /// Метод поражения в игре
    /// </summary>
    private void DefeatGame()
    {
        _defeatGame.SetActive(true);
        _raidTimer.OnOffTimer = false;
        _eatingTimer.OnOffTimer = false;
        _harvestTimer.OnOffTimer = false;
        _raidTimer.Tick = false;
        _eatingTimer.Tick = false;
        _harvestTimer.Tick = false;
        hireWarriorButton.interactable = false;
        hirePeasantButton.interactable = false;        
    }

    /// <summary>
    /// Метод победы в игре
    /// </summary>
    private void VictoryGame()
    {
        _victoryGame.SetActive(true);
        _raidTimer.OnOffTimer = false;
        _eatingTimer.OnOffTimer = false;
        _harvestTimer.OnOffTimer = false;
        _raidTimer.Tick = false;
        _eatingTimer.Tick = false;
        _harvestTimer.Tick = false;
        hireWarriorButton.interactable = false;
        hirePeasantButton.interactable = false;
    }        
}
