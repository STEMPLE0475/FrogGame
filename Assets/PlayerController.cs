using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    [SerializeField] PhotonView PV;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine) rigidbody.AddForce(new Vector3(Input.GetAxisRaw("Horizontal")*Time.deltaTime *2f, 0f, Input.GetAxisRaw("Vertical") * 2f * Time.deltaTime), ForceMode.Impulse);
    }
}
