-- Observera att innehållet i denna fil är bara för att hålla koll på
-- kolumnerna och tabellerna.

-- Tabell för client med 4 kolumner 
CREATE TABLE clients (
    id SERIAL PRIMARY KEY, 
    username VARCHAR(255) NOT NULL,
    passwordhash VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL 
);

-- Värde input 
INSERT INTO clients (username, passwordhash, email) 
VALUES ('test1', 'foreign123', 'key'); 

-- Tabell för transaktioner med 4 kolumner 
CREATE TABLE transactions ( 
    client_id INT NOT NULL, 
    transaction_date DATE NOT NULL, 
    amount DECIMAL(10, 2) NOT NULL, 
    description TEXT, 
    CONSTRAINT fk_client FOREIGN KEY (client_id) REFERENCES clients(id) 
   
);

-- Värde input 
INSERT INTO transactions (client_id, amount, description) 
VALUES (1, 100.00, 'Loan');