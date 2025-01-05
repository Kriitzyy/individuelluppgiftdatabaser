
sql
Kopiera kod
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    passwordhash VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL
);

INSERT INTO clients (username, passwordhash, email) 
VALUES ('test1', 'foreign123', 'key');

CREATE TABLE transactions (
    client_id INT PRIMARY KEY,
    transaction_date DATE NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    description TEXT,
    CONSTRAINT fk_client FOREIGN KEY (client_id) REFERENCES users (id)
);

INSERT INTO transactions (client_id, amount, description, transaction_type) 
VALUES (1, 100.00, 'Loan', 'income');

// Observera att filen och innehållet är bara för att hålla koll på
// columner och tabellerna.