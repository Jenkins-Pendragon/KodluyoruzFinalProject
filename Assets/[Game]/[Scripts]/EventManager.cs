using UnityEngine.Events;
public static class EventManager
{
    // Joystick->Fullscreen tap içinde kullanıldı. Ekrana Tıklana ve Kaldırma Anlarında Invokelandı.
    // Helper'i Kapatmak gibi işlerde OnEnable içinde AddListener ile Event'i dinleyebilir ve gerekli metotları ortama fırlatabilirsiniz.
    public static UnityEvent OnTapRelease = new UnityEvent();
    public static UnityEvent OnTapStart = new UnityEvent();

    // GameManager -> Awake içinde çağrıldı
    public static UnityEvent OnGameStart = new UnityEvent(); 
    // LevelManager içinde oyunun yapısına göre invoke'layınız
    public static UnityEvent OnGameOver = new UnityEvent(); 
}
