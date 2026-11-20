using UnityEngine; // Traz as classes da Unity (MonoBehaviour, Physics2D, Gizmos, Input...)

// Ataque em área do Player: ao apertar Espaço, causa dano a todos os
// inimigos dentro de um círculo ao redor dele, respeitando uma recarga.
// Este script fica no GameObject do Player.
public class PlayerAttack : MonoBehaviour
{
    // ===== CONFIGURAÇÕES (editáveis no Inspector) =====

    // Quanto tempo (em segundos) o Player espera entre um ataque e outro.
    // É a DURAÇÃO da recarga, um valor fixo.
    [SerializeField] private float attackCooldown = 0.5f;

    // Raio do círculo de ataque, em unidades do mundo (1 unidade ≈ 1 tile).
    // O círculo vermelho desenhado pelo Gizmo mostra esse alcance na janela Scene.
    [SerializeField] private float attackRange = 1f;

    // Quanto de vida cada ataque tira de cada inimigo atingido.
    [SerializeField] private int attackDamage = 1;

    // Quais camadas (Layers) o ataque pode atingir.
    // No Inspector, marque SÓ "Enemy", para não acertar o Player, corações ou o cenário.
    // Por dentro é uma "máscara de bits": cada camada é um bit ligado ou desligado.
    [SerializeField] private LayerMask enemyLayer;

    // ===== ESTADO (muda durante o jogo, não aparece no Inspector) =====

    // QUANTO FALTA para poder atacar de novo. Começa em 0 (pode atacar logo).
    // Diferente do attackCooldown: este diminui a cada frame, aquele é fixo.
    private float cooldownTimer;

    // Update: a Unity chama uma vez por frame. É aqui que se lê o teclado,
    // porque GetKeyDown é um "evento" que dura só um frame e o FixedUpdate poderia perdê-lo.
    private void Update()
    {
        // Se ainda falta tempo de recarga, desconta o tempo que passou neste frame.
        // Time.deltaTime = segundos desde o frame anterior (ex.: 0,016 a 60 fps).
        // Somando isso todo frame, contamos tempo real, independente do fps.
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Ataca SE: apertou Espaço NESTE frame (GetKeyDown = um aperto, um ataque)
        //       E (&&) a recarga já acabou.
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0)
        {
            Attack();                        // executa o golpe
            cooldownTimer = attackCooldown;  // e só AGORA reinicia a recarga
        }
    }

    // Attack: O QUE o ataque faz. (O Update decide QUANDO; aqui decide O QUÊ.)
    private void Attack()
    {
        // Pergunta à física: "quais colliders da camada Enemy estão dentro
        // deste círculo AGORA?". Parâmetros: centro, raio, camadas.
        // transform.position = posição do Player (o script está no próprio Player).
        // Devolve um ARRAY (Collider2D[]), vazio se não acertou ninguém.
        // É uma consulta instantânea: só LÊ a física, não move nada, por isso
        // pode rodar no Update.
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        // hits.Length = quantos colliders foram encontrados.
        Debug.Log("Player atacou! Inimigos atingidos: " + hits.Length);

        // Para CADA collider atingido... (igual ao "for (x : lista)" do Java)
        foreach (Collider2D hit in hits)
        {
            // ...tenta pegar o componente EnemyHealth daquele objeto.
            // TryGetComponent devolve true se encontrou, e entrega o componente
            // pela variável "out enemyHealth". Se o objeto não tiver EnemyHealth,
            // simplesmente pula (sem erro).
            if (hit.TryGetComponent(out EnemyHealth enemyHealth))
            {
                // Pede para o inimigo tomar dano. O EnemyHealth decide o resto
                // (diminuir a vida, morrer, sumir da cena).
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    // Gizmos: desenhos que aparecem SÓ no editor (janela Scene), nunca no jogo final.
    // "Selected" = só desenha quando o Player está selecionado na Hierarchy.
    // Serve para ver e ajustar o alcance do ataque visualmente.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;                                  // cor do desenho
        Gizmos.DrawWireSphere(transform.position, attackRange);    // contorno do círculo de ataque
    }
}
