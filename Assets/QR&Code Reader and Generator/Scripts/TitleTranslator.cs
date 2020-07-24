
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleTranslator : MonoBehaviour
{
    public Text Title;
    // Start is called before the first frame update
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "QRScanScene") { 
           int id = LangEngine.Lan_ID;
            switch (id)
            {
                case 1:
                    Title.text = "Aim the QR code";
                    QRDecodeTest.m_errore = "Unrecognized code press reset and retry";
                    break;
                case 2:
                    Title.text = "Inquadra il codice QR";
                    QRDecodeTest.m_errore = "Codice non riconosciuto premi reset e ritenta";
                    break;
                case 3:
                    Title.text = "Вставьте QR-код";
                    QRDecodeTest.m_errore = "Нераспознанный код нажмите сброс и повторите попытку";
                    break;
                case 4:
                    Title.text = "Enmarca el código QR";
                    QRDecodeTest.m_errore = "Código no reconocido presione reiniciar y vuelva a intentar";
                    break;
                case 5:
                    Title.text = "Rahmen Sie den QR-Code";
                    QRDecodeTest.m_errore = "Nicht erkannter Code Drücken Sie Zurücksetzen und versuchen Sie es erneut";
                    break;
                case 6:
                    Title.text = "Encadrez le code QR";
                    QRDecodeTest.m_errore = "Code non reconnu, appuyez sur reset et réessayez";
                    break;
                default:
                    Title.text = "Inquadra il codice QR con disegnata la forchetta";
                    QRDecodeTest.m_errore = "Codice non riconosciuto premi reset e ritenta";
                    break;
            }
        }
        
    }
    public void MainSetLanguage()
    {
        
            int id = LangEngine.Lan_ID;
            switch (id)
            {
                case 1:
                    Title.text = "Press the scan button and frame with your mobile phone camera the QRCODE placed on the table";
                    QRDecodeTest.m_errore = "Unrecognized code press reset and retry";
                    break;
                case 2:
                    Title.text = "Premi il tasto scan ed inquadra con la fotocamera del tuo cellulare il QRCODE posto sul tavolo";
                    QRDecodeTest.m_errore = "Codice non riconosciuto premi reset e ritenta";
                    break;
                case 3:
                    Title.text = "Нажмите кнопку сканирования и кадр с камерой вашего мобильного телефона QRCODE размещен на столе";
                    QRDecodeTest.m_errore = "Нераспознанный код нажмите сброс и повторите попытку";
                    break;
                case 4:
                    Title.text = "Presione el botón de escaneo y marco con la cámara de su teléfono móvil el QRCODE colocado sobre la mesa";
                    QRDecodeTest.m_errore = "Código no reconocido presione reiniciar y vuelva a intentar";
                    break;
                case 5:
                    Title.text = "Drücken Sie die Scan-Taste und Rahmen mit Ihrer Handykamera der QRCODE auf den Tisch gelegt";
                    QRDecodeTest.m_errore = "Nicht erkannter Code Drücken Sie Zurücksetzen und versuchen Sie es erneut";
                    break;
                case 6:
                    Title.text = "Appuyez sur le bouton scan et cadre avec la caméra de votre téléphone mobile le QRCODE placé sur la table";
                    QRDecodeTest.m_errore = "Code non reconnu, appuyez sur reset et réessayez";
                    break;
                default:
                    Title.text = "Premi il tasto scan ed inquadra con la fotocamera del tuo cellulare il QRCODE posto sul tavolo";
                    QRDecodeTest.m_errore = "Codice non riconosciuto premi reset e ritenta";
                    break;
            }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
