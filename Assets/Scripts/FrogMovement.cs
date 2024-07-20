using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Rigidbody2D rb;
    public float avoidanceRadius = 0.5f;
    public LayerMask obstacleLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;  // Вимкнути гравітацію для 2D персонажа
        rb.freezeRotation = true;  // Заборонити обертання персонажа
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;

            // Перевірка, чи натиснута точка знаходиться всередині колайдера
            Collider2D clickedCollider = Physics2D.OverlapPoint(clickPosition, obstacleLayer);
            if (clickedCollider == null)
            {
                targetPosition = clickPosition;
                isMoving = true;
            }
        }

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

        // Перевірка на наявність перешкод
        Collider2D obstacle = Physics2D.OverlapCircle(newPosition, avoidanceRadius, obstacleLayer);
        if (obstacle == null)
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            // Обхід перешкоди по напрямку до цілі
            Vector2 avoidanceDirection = GetAvoidanceDirection(direction, obstacle);
            Vector2 avoidancePosition = rb.position + avoidanceDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(avoidancePosition);
        }

        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }

    Vector2 GetAvoidanceDirection(Vector2 direction, Collider2D obstacle)
    {
        Vector2 obstaclePosition = obstacle.transform.position;
        Vector2 avoidanceDirection = (rb.position - (Vector2)obstaclePosition).normalized;

        // Перевірка чи новий напрямок приведе до цілі
        Vector2 newPosition = rb.position + avoidanceDirection * moveSpeed * Time.fixedDeltaTime;
        if (Physics2D.OverlapCircle(newPosition, avoidanceRadius, obstacleLayer) == null)
        {
            return avoidanceDirection;
        }

        // Альтернативний напрямок обходу
        return -avoidanceDirection;
    }
}
