using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    private Image _img;
    [SerializeField] private float _maxTime;
    private float _currentTime;
    internal bool Tick = true;
    internal bool OnOffTimer = true;

    public float MaxTime
    {
        get
        {
            return _maxTime;
        }        
    }   
    
    public float CurrentTime
    {        
        set
        {
            _currentTime = _maxTime;
        }
    }
            
    private void Start()
    {
        _img = GetComponent<Image>();
        _currentTime = _maxTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (OnOffTimer == true)
        {
            Tick = false;
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
            {
                Tick = true;
                _currentTime = _maxTime;
            }
            _img.fillAmount = _currentTime / _maxTime;
        }        
    }
}
