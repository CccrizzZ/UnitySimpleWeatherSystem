
using System;
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

                x = UnityEngine.Random.Range(1,100);
                Debug.Log(x);

                if (x < OverCastPossibility)
                {
                    Debug.Log("Overcast");
                    OverCast();
                    
                }
            }
        }
    }

    IEnumerator ChangeColor(Color TargetColor)
    {

        yield return null;

        globalLight.color = Color.Lerp(globalLight.color, TargetColor, 1.0f);
    }


    void OverCast()
    {
        StartCoroutine(GetOverCast());
    }
    IEnumerator GetOverCast()
    {
        StartCoroutine(FadeOut(SoundArray[0]));
        RandWeather = false;

        // while (globalLight.intensity > 0.8f)
        // {
        //     globalLight.intensity -= 0.0001f;
        // }

        // globalLight.intensity = Mathf.Lerp(globalLight.intensity, 0.8f, 5.0f);
        // myLerp(50.0f, 0.8f);
        

        // globalLight.color = Color.Lerp(globalLight.color, new Color(0.84f, 0.62f, 1.0f, 1.0f), 5);
        StartCoroutine(ChangeColor(new Color(0.84f, 0.62f, 1.0f, 1.0f)));


        yield return new WaitForSeconds(10);


        // globalLight.intensity = Mathf.Lerp(globalLight.intensity, 0.8f, 5.0f);


        // while (globalLight.intensity <= 0.5f)
        // {
        //     globalLight.intensity += 0.0001f;
        // }

        if(UnityEngine.Random.Range(1,100)>RainPossibility)
        {
            Rain();
        }
        else
        {
            StartCoroutine(ChangeColor(startColor));
            // globalLight.color = Color.Lerp(globalLight.color, startColor, 1.0f);
            
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


        StartCoroutine(ChangeColor(new Color(0.84f, 0.62f, 1.0f, 1.0f)));

        var temp = Instantiate(rain);
        StartCoroutine(FadeIn(SoundArray[1]));


        yield return new WaitForSeconds(10);
        

        
        // globalLight.color = startColor;


        while (globalLight.intensity < 1.0f)
        {
            globalLight.intensity += Time.deltaTime * 0.0001f;
        }

        Destroy(temp);
        
        if(UnityEngine.Random.Range(1,100)>ThunderStormPossibility)
        {
            ThunderStorm();
        }
        else{
            StartCoroutine(FadeOut(SoundArray[1]));
            StartCoroutine(ChangeColor(startColor));
            StartCoroutine(FadeIn(SoundArray[0]));
            RandWeather = true;
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
            if(!RandWeather)
            {
                yield return new WaitForSeconds(randInterval);
                var pos = new Vector2(UnityEngine.Random.Range(-10,10), UnityEngine.Random.Range(-10,10));
                thunder.transform.position = pos;
                var temp = Instantiate(thunder);
                thunder.GetComponent<AudioSource>().Play();

                yield return new WaitForSeconds(1.0f);
                Destroy(temp.GetComponent<SpriteRenderer>());
                yield return new WaitForSeconds(3.0f);
                Destroy(temp);

            }


            
        }


    }


    // IEnumerator PlayThunderSound()
    // {
    //     yield return new WaitForSeconds(1);
    //     if(!RandWeather)
    //     {
    //         SoundArray[2].Play();
    //     }
    // }

    void ThunderStorm()
    {
        
        StartCoroutine(GetThunderStorm());
    }

    IEnumerator GetThunderStorm()
    {
        StartCoroutine(FadeOut(SoundArray[0]));
        RandWeather = false;

        var temp = Instantiate(rain);

        if (!SoundArray[1].isPlaying)
        {
            StartCoroutine(FadeIn(SoundArray[1]));
        }

        StartCoroutine(FadeIn(SoundArray[3]));

        // while (globalLight.intensity > 0.5f)
        // {
        //     globalLight.intensity -= 0.1f / Time.deltaTime;
        // }

        globalLight.intensity = Mathf.Lerp(globalLight.intensity, 0.5f, 5.0f);

        globalLight.color = Color.Lerp(globalLight.color, new Color(0.14f, 0.05f, 0.57f, 1.0f), 1);
       
        StartCoroutine(GetThunder(UnityEngine.Random.Range(0.1f, 5.0f)));


        yield return new WaitForSeconds(15);
        StartCoroutine(FadeOut(SoundArray[3]));
        

        StartCoroutine(FadeOut(SoundArray[1]));
        // while (globalLight.intensity < 1.0f)
        // {
        //     globalLight.intensity += 0.1f / Time.deltaTime;
        // }
        
        globalLight.intensity = Mathf.Lerp(globalLight.intensity, 1.0f, 5.0f);

        globalLight.color = Color.Lerp(globalLight.color, startColor, 1);

        Destroy(temp);
        RandWeather = true;
        StartCoroutine(FadeIn(SoundArray[0]));
        

    }

    IEnumerator FadeOut(AudioSource s)
    {
        float StartVolume = s.volume;
        while (s.volume > 0.1f)
        {
            s.volume -= StartVolume * Time.deltaTime / 1.0f;
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


    void myLerp(float duration, float val)
    {

        float temp = 0;
        if (temp < duration)
        {
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, val, temp / duration);
            temp += Time.deltaTime;
        }
    }
}
