using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoSceneGameBefore : MonoBehaviour
{
    public GameObject Level1;

    private bool IsCanBeganOneLevel = false;

    private Ray ray;

    public GameObject[] LevelUI;


    private AudioSource audioSource;
    private AudioClip audioClip;

    private bool IsBegan = false;

    public Sprite bg2;
    public Sprite bg1;

    public GameObject[] big;

    void Start ()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioClip = Resources.Load<AudioClip>("SE_001");
        audioSource.clip = audioClip;

    }
	
	void Update ()
    {
        RaycastHit hit;
        Vector2 v = new Vector2(Screen.width / 2, Screen.height / 2); //屏幕中心点
        if (Physics.Raycast(Camera.main.ScreenPointToRay(v), out hit))
        {

            //其他操作

            #region UI界面放大效果和UI点击音效

            if (hit.collider.gameObject.name == "Level1cube")
            {
                print("碰到第一关的UI了");

                LevelUI[0].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
                LevelUI[0].gameObject.GetComponent<AudioSource>().enabled = true;

                LevelUI[0].gameObject.GetComponent<Image>().sprite = bg2;

                big[0].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
                IsCanBeganOneLevel = true;
            }
            else
            {
                IsCanBeganOneLevel = false;
               
                LevelUI[0].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                LevelUI[0].gameObject.GetComponent<AudioSource>().enabled = false;

                LevelUI[0].gameObject.GetComponent<Image>().sprite = bg1;
                big[0].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }

            if (hit.collider.gameObject.name == "Level2cube")
            {
                LevelUI[1].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                LevelUI[1].gameObject.GetComponent<AudioSource>().enabled = true;
                LevelUI[1].gameObject.GetComponent<Image>().sprite = bg2;
                big[1].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                LevelUI[1].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.64f, 0.64f, 0.64f);
                LevelUI[1].gameObject.GetComponent<AudioSource>().enabled = false;
                LevelUI[1].gameObject.GetComponent<Image>().sprite = bg1;
                big[1].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }

            if (hit.collider.gameObject.name == "Level3cube")
            {

               
                LevelUI[2].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                LevelUI[2].gameObject.GetComponent<AudioSource>().enabled = true;
                LevelUI[2].gameObject.GetComponent<Image>().sprite = bg2;
                big[2].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                
                LevelUI[2].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.64f, 0.64f, 0.64f);
                LevelUI[2].gameObject.GetComponent<AudioSource>().enabled = false;
                LevelUI[2].gameObject.GetComponent<Image>().sprite = bg1;
                big[2].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }

            if (hit.collider.gameObject.name == "Level4cube")
            {
                LevelUI[3].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                LevelUI[3].gameObject.GetComponent<AudioSource>().enabled = true;
                LevelUI[3].gameObject.GetComponent<Image>().sprite = bg2;
                big[3].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                
                LevelUI[3].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.64f, 0.64f, 0.64f);
                LevelUI[3].gameObject.GetComponent<AudioSource>().enabled = false;
                LevelUI[3].gameObject.GetComponent<Image>().sprite = bg1;
                big[3].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }

            if (hit.collider.gameObject.name == "Level5cube")
            {
                LevelUI[4].gameObject.GetComponent<AudioSource>().enabled = true;
                LevelUI[4].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                LevelUI[4].gameObject.GetComponent<Image>().sprite = bg2;
                big[4].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                LevelUI[4].gameObject.GetComponent<AudioSource>().enabled = false;
                LevelUI[4].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.64f, 0.64f, 0.64f);
                LevelUI[4].gameObject.GetComponent<Image>().sprite = bg1;
                big[4].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }






            if (hit.collider.gameObject.name == "Level6cube")
            {
                LevelUI[9].gameObject.GetComponent<AudioSource>().enabled = true;
                LevelUI[9].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                LevelUI[9].gameObject.GetComponent<Image>().sprite = bg2;
                big[9].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                LevelUI[9].gameObject.GetComponent<AudioSource>().enabled = false;
                LevelUI[9].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.64f, 0.64f, 0.64f);
                LevelUI[9].gameObject.GetComponent<Image>().sprite = bg1;
                big[9].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }



            if (hit.collider.gameObject.name == "Level7cube")
            {
                LevelUI[8].gameObject.GetComponent<AudioSource>().enabled = true;
                LevelUI[8].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                LevelUI[8].gameObject.GetComponent<Image>().sprite = bg2;
                big[8].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                LevelUI[8].gameObject.GetComponent<AudioSource>().enabled = false;
                LevelUI[8].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.64f, 0.64f, 0.64f);
                LevelUI[8].gameObject.GetComponent<Image>().sprite = bg1;
                big[8].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }



            if (hit.collider.gameObject.name == "Level8cube")
            {
                LevelUI[7].gameObject.GetComponent<AudioSource>().enabled = true;
                LevelUI[7].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                LevelUI[7].gameObject.GetComponent<Image>().sprite = bg2;
                big[7].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

            }
            else
            {
                LevelUI[7].gameObject.GetComponent<AudioSource>().enabled = false;
                LevelUI[7].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.64f, 0.64f, 0.64f);
                LevelUI[7].gameObject.GetComponent<Image>().sprite = bg1;
                big[7].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }



            if (hit.collider.gameObject.name == "Level9cube")
            {
                LevelUI[6].gameObject.GetComponent<AudioSource>().enabled = true;
                LevelUI[6].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                LevelUI[6].gameObject.GetComponent<Image>().sprite = bg2;
                big[6].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                LevelUI[6].gameObject.GetComponent<AudioSource>().enabled = false;
                LevelUI[6].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.64f, 0.64f, 0.64f);
                LevelUI[6].gameObject.GetComponent<Image>().sprite = bg1;
                big[6].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }



            if (hit.collider.gameObject.name == "Level10cube")
            {

                LevelUI[5].gameObject.GetComponent<AudioSource>().enabled = true;
                LevelUI[5].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                LevelUI[5].gameObject.GetComponent<Image>().sprite = bg2;
                big[5].gameObject.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                LevelUI[5].gameObject.GetComponent<AudioSource>().enabled = false;
                LevelUI[5].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.64f, 0.64f, 0.64f);
                LevelUI[5].gameObject.GetComponent<Image>().sprite = bg1;
                big[5].gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }
        }
#endregion

        if (IsCanBeganOneLevel && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.JoystickButton0)))
        {
            //SceneManager.LoadScene("Main");
            IsBegan = true;
            //SceneManager.LoadScene("GameBeforeScene");
            GameObject.Find("CanvasContent").GetComponent<InterfaceAnimManager>().startDisappear();
        }


        if (IsBegan)
        {
            //LevelUI[0].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);
            //LevelUI[1].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);
            //LevelUI[2].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);
            //LevelUI[3].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);
            //LevelUI[4].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);
            //LevelUI[5].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);

            //LevelUI[6].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);
            //LevelUI[7].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);
            //LevelUI[8].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);
            //LevelUI[9].GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * 30);
            StartCoroutine(NewScene());
            
        }
    }

    IEnumerator NewScene()
    {
        //yield return new WaitForSeconds(0.4f);
        //LevelUI[0].gameObject.SetActive(false);
        //LevelUI[1].gameObject.SetActive(false);
        //LevelUI[2].gameObject.SetActive(false);
        //LevelUI[3].gameObject.SetActive(false);
        //LevelUI[4].gameObject.SetActive(false);
        //LevelUI[5].gameObject.SetActive(false);
        //LevelUI[6].gameObject.SetActive(false);
        //LevelUI[7].gameObject.SetActive(false);
        //LevelUI[8].gameObject.SetActive(false);
        //LevelUI[9].gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameBeforeScene");
    }
   
}
