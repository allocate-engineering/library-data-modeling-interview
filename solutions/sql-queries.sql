-- Library Management System - SQL Query Solutions
-- These are the expected solutions for Part 1 of the interview

-- 1. All books currently checked out
SELECT 
    u.name as user_name, 
    u.library_card_number,
    b.title, 
    b.isbn,
    bc.copy_number, 
    c.checkout_date, 
    c.due_date,
    CASE 
        WHEN c.due_date < CURRENT_DATE THEN 'OVERDUE'
        ELSE 'ON TIME'
    END as status
FROM checkouts c
JOIN users u ON c.user_id = u.id
JOIN book_copies bc ON c.book_copy_id = bc.id
JOIN books b ON bc.book_id = b.id
WHERE c.is_returned = false
ORDER BY c.due_date;

-- 2. Users with overdue books (as of 2025-09-08)
SELECT DISTINCT
    u.name,
    u.library_card_number,
    u.email,
    COUNT(c.id) as overdue_books
FROM users u
JOIN checkouts c ON u.id = c.user_id
WHERE c.is_returned = false 
    AND c.due_date < '2025-09-08'
GROUP BY u.id, u.name, u.library_card_number, u.email
ORDER BY overdue_books DESC;

-- 3. Most popular authors (by total checkouts)
SELECT 
    a.name as author_name,
    a.nationality,
    COUNT(c.id) as total_checkouts,
    COUNT(DISTINCT b.id) as unique_books
FROM authors a
JOIN book_authors ba ON a.id = ba.author_id
JOIN books b ON ba.book_id = b.id
JOIN book_copies bc ON b.id = bc.book_id
JOIN checkouts c ON bc.id = c.book_copy_id
GROUP BY a.id, a.name, a.nationality
ORDER BY total_checkouts DESC;

-- Additional useful queries:

-- 4. Available book copies by ISBN
SELECT 
    b.isbn,
    b.title,
    bc.copy_number,
    bc.condition,
    CASE 
        WHEN c.id IS NULL THEN 'AVAILABLE'
        ELSE 'CHECKED OUT'
    END as availability
FROM books b
JOIN book_copies bc ON b.id = bc.book_id
LEFT JOIN checkouts c ON bc.id = c.book_copy_id AND c.is_returned = false
ORDER BY b.title, bc.copy_number;

-- 5. Users who can/cannot check out books
SELECT 
    u.name,
    u.library_card_number,
    COUNT(CASE WHEN c.due_date < CURRENT_DATE THEN 1 END) as overdue_count,
    CASE 
        WHEN COUNT(CASE WHEN c.due_date < CURRENT_DATE THEN 1 END) = 0 THEN 'CAN CHECKOUT'
        ELSE 'BLOCKED - HAS OVERDUE'
    END as checkout_status
FROM users u
LEFT JOIN checkouts c ON u.id = c.user_id AND c.is_returned = false
GROUP BY u.id, u.name, u.library_card_number
ORDER BY u.name;

-- 6. Late fee calculation example
SELECT 
    u.name,
    b.title,
    c.due_date,
    CURRENT_DATE as today,
    GREATEST(0, CURRENT_DATE - c.due_date) as days_overdue,
    GREATEST(0, CURRENT_DATE - c.due_date) * 0.50 as late_fee
FROM checkouts c
JOIN users u ON c.user_id = u.id
JOIN book_copies bc ON c.book_copy_id = bc.id
JOIN books b ON bc.book_id = b.id
WHERE c.is_returned = false
    AND c.due_date < CURRENT_DATE;