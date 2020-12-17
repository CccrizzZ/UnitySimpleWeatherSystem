
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;
public class WeatherController : MonoBehaviour
{


    [Header("Parameter")]
    public float RainTime;
    public float ThunderStormTime;
    public float OverCastTime;
    public float RainPossibility;
    public float ThunderStormPossibility;
    public float OverCastPossibility;
    public float speed;



    [Header("Lights")]
    public Light2D globalLight;
    
    [Header("Weather Prefab")]
    public GameObject rain;
    public GameObject thunder;
    public GameObject overcast;
    






    [Header("Sounds")]  
    public AudioSource[] SoundArray;
    int x = 0;
    bool RandWeather = true;

    Color startColor;
    // Color colorValue;
    enum SoundArrayIndex
    {
        BIRDS,
        RAIN,
        THUNER,
        WIND,
    }


    void Start()
    {
        if (RainTime == 0)
        {
            RainTime = 10.0f;
        }


        startColor = globalLight.color;
        StartCoroutine(FadeIn(SoundArray[0]));
    }

    void Update()
    {
        
        if (Time.frameCount % (1800 / speed) == 0)
        {
            if (RandWeather)
            {

                x = Random.Range(1,100);
                Debug.Log(x);

                if (x < ThunderStormPossibility)
                {
                    Debug.Log("ThunderStorm");
                    ThunderStorm();
                }
                else if ( x < RainPossibility)
                {
                    Debug.Log("Rain");
                    Rain();
                }
                else if (x < OverCastPossibility)
                {
                    Debug.Log("Overcast");
                    OverCast();
                    
                }

            }
        
        
        
        }
    }



    void OverCast()
    {
        StartCoroutine(GetOverCast());
    }
    IEnumerator GetOverCast()
    {
        StartCoroutine(FadeOut(SoundArray[0]));
        RandWeather = false;

        while (globalLight.intensity > 0.8f)
        {
            globalLight.intensity -= Time.deltaTime * 0.0001f;
        }

        globalLight.color = Color.Lerp(globalLight.color, new Color(0.9f, 0.9f, 1.0f, 1.0f), 1);



        yield return new WaitForSeconds(10);


        while (globalLight.intensity <= 0.5f)
        {
            globalLight.intensity += Time.deltaTime * 0.0001f;
        }

        if(Random.Range(1,100)>50)
        {
            Rain();
        }
        else
        {
            globalLight.color = Color.Lerp(globalLight.color, startColor, 2);
            
            RandWeather = true;
            StartCoroutine(FadeIn(SoundArray[0]));

        }

        


    }

    void Rain()
    {
        StartCoroutine(GetRain());
    }

    IEnumerator GetRain()
    {
        StartCoroutine(FadeOut(SoundArray[0]));
        RandWeather = false;

        while (globalLight.intensity > 0.5f)
        {
            globalLight.intensity -= Time.deltaTime * 0.0001f;
        }
        globalLight.color = Color.Lerp(globalLight.color, new Color(0.84f, 0.62f, 1.0f, 1.0f), 5);
       
        var temp = Instantiate(rain);
        StartCoroutine(FadeIn(SoundArray[1]));


        yield return new WaitForSeconds(10);
        

        
        StartCoroutine(FadeOut(SoundArray[1]));
        // globalLight.color = startColor;


        while (globalLight.intensity < 1.0f)
        {
            globalLight.intensity += Time.deltaTime * 0.0001f;
        }

        Destroy(temp);
        
        if(Random.Range(1,100)>50)
        {
            ThunderStorm();
        }
        else{
            globalLight.color = Color.Lerp(globalLight.color, startColor, 5);
            RandWeather = true;
            StartCoroutine(FadeIn(SoundArray[0]));
        }

        


    }

    // void Thunder()
    // {
    //     StartCoroutine(GetThunder());
    // }

    IEnumerator GetThunder(float randInterval)
    {
        for (var i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(randInterval);
            var pos = new Vector2(Random.Range(-10,10), Random.Range(-10,10));
            thunder.transform.position = pos;
            var temp = Instantiate(thunder);
            StartCoroutine(PlayThunderSound());
            yield return new WaitForSeconds(0.5f);
            Destroy(temp);
        }


    }


    IEnumerator PlayThunderSound()
    {
        yield return new WaitForSeconds(1);
        if(!RandWeather)
        {
            SoundArray[2].Play();
        }
    }

    void ThunderStorm()
    {
        
        StartCoroutine(GetThunderStorm());
    }

    IEnumerator GetThunderStorm()
    {
        StartCoroutine(FadeOut(SoundArray[0]));
        RandWeather = false;

        var temp = Instantiate(rain);
        StartCoroutine(FadeIn(SoundArray[1]));

        while (globalLight.intensity > 0.5f)
        {
            globalLight.intensity -= Time.deltaTime * 0.0001f;
        }

        globalLight.color = Color.Lerp(globalLight.color, new Color(0.14f, 0.05f, 0.57f, 1.0f), 2);
       
        StartCoroutine(GetThunder(Random.Range(0.1f, 5.0f)));


        yield return new WaitForSeconds(15);
        

        StartCoroutine(FadeOut(SoundArray[1]));
        while (globalLight.intensity < 1.0f)
        {
            globalLight.intensity += Time.deltaTime * 0.0001f;
        }
        
        globalLight.color = Color.Lerp(globalLight.color, startColor, 2);

        Destroy(temp);
        RandWeather = true;
        StartCoroutine(FadeIn(SoundArray[0]));
        

    }

    IEnumerator FadeOut(AudioSource s)
    {
        float StartVolume = s.volume;
        while (s.volume < 0.1f)
        {
            s.volume -= StartVolume * Time.deltaTime / 0.8f;
            yield return null;
        }
        s.Stop();
    }
    
    IEnumerator FadeIn(AudioSource s)
    {
        s.volume = 0;
        s.Play();
        float StartVolume = 0.1f;
        while (s.volume < 1.0f)
        {
            s.volume += StartVolume * Time.deltaTime / 1.0f;
            yield return null;
        }
    }
    // IEnumerator ChangeColor(Color start, Color end, float duration) 
    // {
        
    //     for (float t=0f;t<duration;t+=Time.deltaTime) 
    //     {
    //         float normalizedTime = t/duration;
    //         globalLight.color = Color.Lerp(start, end, normalizedTime);
    //         yield return null;
    //     }
    //     globalLight.color = end; 
    // }
}
