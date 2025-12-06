using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponCard : MonoBehaviour
{
    // Start is called before the first frame update
    //チャージは一旦無効化する
    [Title("Status")]
    public float recoilTime = 0.7f;
    public float chargeTime = 0.5f;
    public float chargeMax = 100;
    public float defaultCharge = 25;
    public float POW = 3, bulletPenetrate = 1, bulletSize = 1, bulletDamage = 10, bulletnum = 1;


    //内部処理に使う変数
    private Vector3 _clickedPosition, _releasedPosition;
    private Vector3 _shotVec,_targetPos;
    private Vector3 arrowDefaultPosition;
    private bool isCharging = false, isShot = false, isMouseOver = false;
    //private Vector3 _defaultBulletSize = new Vector3(0.15f, 0.15f, 1);
    //*bulletsizeは見た目でなく判定にのみ影響する
    //private bool oneShot_autoChargeStart = false, oneShot_chargedSoundPlay = false;
    //他オブジェクトを参照
    [Title("reference")]
    [SerializeField] private Transform arrowTip;
    [SerializeField] private GameObject arrow;
    [SerializeField] Transform _Card;
    [SerializeField] float maxSpeed = 0.6f;
    [SerializeField] public AudioSource audioSource;

    [ShowInInspector, LabelText("再生開始位置")]
    [PropertyRange(0, nameof(MaxPlaybackTime))]
    public float sourceStartPos;
    private float MaxPlaybackTime => audioSource != null && audioSource.clip != null ? audioSource.clip.length : 0f;
    private float _charge = 0.0f;

    

    [Title("Shooted")]
    public float charge;
    public float basicDamage = 1;
    public float Penetrate = 1;
    private Vector3 direction;
    void Start()
    {
        arrowDefaultPosition = arrow.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && StaticData.IsPlayerLiving == false && isCharging == true && isMouseOver == false)
        {
            _shotVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _clickedPosition;
            float power = _shotVec.magnitude * POW;
            arrow.transform.localScale = new Vector2(0.3f, power / 7+0.3f);
            //arrow.transform.localPosition = arrowDefaultPosition - new Vector3(0, power / 14);
            // if (power < chargeMax)
            // {
            //     _charge = power;

            // }
            // else
            // {
            //     _charge = chargeMax;
            // }
            _charge = defaultCharge;
            //arrow.transform.position = _positionAt+_shotVec.magnitude / 2;
            arrow.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(_shotVec.x, _shotVec.y) * Mathf.Rad2Deg - 180);
            _Card.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(_shotVec.x, _shotVec.y) * Mathf.Rad2Deg - 180);

        }
        if (Input.GetMouseButtonUp(1) && StaticData.IsPlayerLiving == false && isCharging == true)//発射
        {
            if (isMouseOver == false)
            {
                _releasedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Debug.Log($"released:{_releasedPosition}");

                //目標地点の確定
                Vector3 baseDir = (arrowTip.position - transform.position).normalized;
                float spreadAngle = 30f; // 1発ごとの角度間隔
                int n = (int)bulletnum;

                for (int i = 0; i < n; i++)
                {
                    float angle = 0;
                    if (n % 2 == 1)
                    {
                        // 奇数：中央0°、左右に等間隔
                        angle = (i - n / 2) * spreadAngle;
                    }
                    else
                    {
                        // 偶数：中央なし、左右対称
                        angle = (i - (n - 1) / 2f) * spreadAngle;
                    }
                    Quaternion rot = Quaternion.Euler(0, 0, angle);
                    Vector3 shotDir = rot * baseDir;
                    Vector3 shotTarget = transform.position + shotDir * (arrowTip.position - transform.position).magnitude;
                    //*チャージ秒数によって挙動変えるときはココ
                    _targetPos = shotTarget;
                    charge = _charge;
                    basicDamage = bulletDamage;
                    Penetrate = bulletPenetrate;
                    Shoot(_targetPos);

                }
            }
                _charge = 0;
                isCharging = false;
                //矢印初期化
                arrow.transform.localScale = new Vector2(0, 0);
                arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (isShot)//TODO DOTweenか何かを使ってもっと気持ちいい動きにする 照準も
        {
            GetComponent<CardBase>().clickable = false;
            //Debug.Log(GetComponent<CardBase>().clickable);
            var newPos = transform.position + direction * maxSpeed * charge * 0.8f * Time.deltaTime;
            transform.position = newPos;
        }
    }
    void OnMouseOver()
    {
        if (isShot == false)
        {
            _targetPos = new Vector3(0, 0, 0);
            arrow.transform.localScale = new Vector2(0,0);
            if (Input.GetMouseButtonDown(1) && StaticData.IsPlayerLiving == false)//チャージ開始
            {
                audioSource.time = sourceStartPos;
                audioSource.Play();
                _clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isCharging = true;
            }
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    void OnMouseEnter()
    {
        isMouseOver = true;
    }
    void OnMouseExit()
    {
        isMouseOver = false;
    }
    void Shoot(Vector3 targetPos)
    {
        //*StartCoroutine(LifeTimer());
        targetPos.z = 0;

        //速度を求める
        direction = targetPos - transform.position;
        direction.z = 0;
        direction.Normalize();

        //向きを求める
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        _Card.rotation = Quaternion.Euler(_Card.rotation.x, _Card.rotation.y,_Card.rotation.z + angle);
        isShot = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {//親の名前で判定しても良いかもね
        if (collision.name.Contains("wall"))
            Destroy(gameObject);
        if (collision.name.Contains("Enemy"))
        {
            //Debug.Log($"{basicDamage},{basicDamage + (charge - 5) / 5}");
            if (collision.gameObject.GetComponent<EnemyCard>())
                collision.gameObject.GetComponent<EnemyCard>().Hit(basicDamage + (charge - 5) / 5);
            Penetrate -= 1;
            if (Penetrate == 0)
                Destroy(gameObject);    
            //TODO*ヒットエフェクトを表示
        }
    }
    // IEnumerator LifeTimer()
    // {
    //     yield return new WaitForSeconds(7);
    //     Destroy(gameObject);
    // }

}
