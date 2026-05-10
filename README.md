# CENG454 HW3 — Core Breach

**Öğrenci:** Mehmet Anıl ÜLKÜ — 220441001
**Ders:** CENG454 (Spring 2026) — HW3
**Engine:** Unity 6, C#

## Proje
Core Breach — 2D top-down "reactor defense" prototipi. Oyuncu, merkezdeki enerji çekirdeğini düşman dalgalarına karşı korur; düşmanlardan düşen upgrade pickup'larıyla silahını katmanlandırır.

## Uygulanan tasarım desenleri
- **Observer** — `Core.Damaged`, `Enemy.Died`, `WaveCompleted`, `UpgradeApplied` event'leri çoklu bağımsız subscriber'a yayın
- **Strategy** — `IMovementStrategy` (Direct/ChasePlayer) + `ITargetingStrategy` (Core/Player)
- **Object Pool** — `Projectile`, `Enemy`, `Pickup` için generic pool
- **Decorator** — `IWeaponBehavior` üstünde silah upgrade stack'i (Pierce, DoubleDamage, Spread, Burst, ChainLightning)
- **Interfaces** — `IDamageable` (Core + Enemy), `IPoolable`, `IPool<T>`

## Oynanış
- WASD hareket, mouse aim, sol-tık ateş
- 5 dalga, ~3-3.5 dk session
- Win: 5. dalga biter ve Core ayakta · Lose: Core HP 0
