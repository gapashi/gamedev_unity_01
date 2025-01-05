using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //definindo variaveis privadas
    private Rigidbody2D rb;
    private float moveX;
    private Animator anim;
    private CapsuleCollider2D colliderPlayer;

    //definindo variavel publica
    public float speed;
    public int addJumps; //pulos adicionais / pulo duplo
    public bool isGrounded;
    public float jumpForce;
    public int life;
    public TextMeshProUGUI textLife;
    public string levelName;
    public GameObject gameOver;
    public GameObject canvasPause;
    public bool isPause;
    public float fallThreshold = -10f; 
    public float fallDeathHeight = -20f;
    private bool isFallingToDeath = false;
    private bool cameraStopped = false;

    void Start() //e carregado uma unica vez, quando a cena e rodada
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveX = Input.GetAxisRaw("Horizontal"); //pega a referencia do teclado para a movimentacao
        colliderPlayer = GetComponent<CapsuleCollider2D>();

        if (PlayerPrefs.HasKey("PlayerLife"))
        {
            life = PlayerPrefs.GetInt("PlayerLife");
        }
        else
        {
            life = 5;
        }
    }

    void Update() //aqui vao as informacoes que mudam sempre, como pontuacao, soma e subtracao de vida, etc
    {
        moveX = Input.GetAxisRaw("Horizontal");
        textLife.text = life.ToString();

        if(life <= 0)
        {
            Die();
        }

        if(Input.GetButtonDown("Cancel"))
        {
            PauseScreen();
        }

        if (transform.position.y < fallThreshold && !isFallingToDeath)
        {
            StartCoroutine(FallToDeath());
        }
    }

    void FixedUpdate() //tudo que tem a ver com fisica e melhor colocar aqui
    {
        Move();
        Attack();


        if(isGrounded == true)
        {
            addJumps = 1; //pulo duplo
            if(Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        else
        {
            if(Input.GetButtonDown("Jump") && addJumps > 0)
            {
                addJumps--;
                Jump();
            }
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        if (moveX > 0) 
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            anim.SetBool("isRun", true);
        }

        if (moveX < 0) //flip para a direita
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            anim.SetBool("isRun", true);
        }

        if (moveX == 0)
        {
            anim.SetBool("isRun", false);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        anim.SetBool("isJump", true);
    }

    void Attack()
    {
        if(Input.GetButtonDown("Attack"))
        {
            anim.Play("attack", -1);
        }
    }

    void PauseScreen()
    {
        if(isPause)
        {
            isPause = false;
            Time.timeScale = 1;
            canvasPause.SetActive(false);
        }
        else
        {
            isPause = true;
            Time.timeScale = 0;
            canvasPause.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPause = false;
        Time.timeScale = 1;
        canvasPause.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    void Die()
    {
        this.enabled = false; 
        colliderPlayer.enabled = false; 
        rb.gravityScale = 0; 
        anim.Play("die", -1); 

        gameOver.SetActive(true);

        Time.timeScale = 0;
    }


    IEnumerator FallToDeath()
    {
        isFallingToDeath = true;
        Debug.Log("Player está caindo para a morte...");


        anim.SetTrigger("fall");

        while (transform.position.y > fallDeathHeight)
        {
            yield return null; 
        }

        if (!cameraStopped)
        {
            Camera.main.transform.parent = null;
            cameraStopped = true;
        }

        Die();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("isJump", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    public void SaveLife()
    {
        PlayerPrefs.SetInt("PlayerLife", life);
        PlayerPrefs.Save();
    }
}
