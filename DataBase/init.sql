-- ==========================================
-- InventarySystem — init.sql
-- Schemas: shared | inventory | sales
-- ==========================================

CREATE SCHEMA IF NOT EXISTS shared;
CREATE SCHEMA IF NOT EXISTS inventory;
CREATE SCHEMA IF NOT EXISTS sales;

-- ==========================================
-- SCHEMA: shared
-- ==========================================

CREATE TABLE shared.global_categories (
    id          SERIAL PRIMARY KEY,
    name        VARCHAR(100) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE shared.global_products (
    id              SERIAL PRIMARY KEY,
    category_id     INT REFERENCES shared.global_categories(id),
    name            VARCHAR(150) NOT NULL,
    brand           VARCHAR(100),
    upc_barcode     VARCHAR(50) UNIQUE,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE shared.companies (
    id          SERIAL PRIMARY KEY,
    name        VARCHAR(100) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE shared.warehouses (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    name        VARCHAR(100) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ==========================================
-- SCHEMA: inventory
-- ==========================================

CREATE TABLE inventory.movement_statuses (
    id          SERIAL PRIMARY KEY,
    code        VARCHAR(20) NOT NULL UNIQUE,
    name        VARCHAR(50) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE inventory.movement_types (
    id          SERIAL PRIMARY KEY,
    code        VARCHAR(20) NOT NULL UNIQUE,
    name        VARCHAR(50) NOT NULL,
    operation   CHAR(1) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE inventory.company_attributes (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    name        VARCHAR(50) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (company_id, name)
);

CREATE TABLE inventory.company_products (
    id                  SERIAL PRIMARY KEY,
    company_id          INT NOT NULL REFERENCES shared.companies(id),
    global_product_id   INT REFERENCES shared.global_products(id),
    local_name_alias    VARCHAR(150),
    wholesale_price     DECIMAL(10,2) DEFAULT 0.00,
    is_active           BOOLEAN DEFAULT TRUE,
    created_at          TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE inventory.company_skus (
    id                      SERIAL PRIMARY KEY,
    company_product_id      INT NOT NULL REFERENCES inventory.company_products(id),
    internal_sku            VARCHAR(50) NOT NULL,
    retail_price            DECIMAL(10,2) DEFAULT 0.00,
    is_active               BOOLEAN DEFAULT TRUE,
    created_at              TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (company_product_id, internal_sku)
);

CREATE TABLE inventory.sku_attribute_values (
    id              SERIAL PRIMARY KEY,
    sku_id          INT NOT NULL REFERENCES inventory.company_skus(id),
    attribute_id    INT NOT NULL REFERENCES inventory.company_attributes(id),
    value           VARCHAR(50) NOT NULL,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (sku_id, attribute_id)
);

CREATE TABLE inventory.batches (
    id                  SERIAL PRIMARY KEY,
    sku_id              INT NOT NULL REFERENCES inventory.company_skus(id),
    batch_number        VARCHAR(50) NOT NULL,
    manufacture_date    DATE,
    expiration_date     DATE,
    is_active           BOOLEAN DEFAULT TRUE,
    created_at          TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (sku_id, batch_number)
);

CREATE TABLE inventory.stocks (
    id              SERIAL PRIMARY KEY,
    warehouse_id    INT NOT NULL REFERENCES shared.warehouses(id),
    sku_id          INT NOT NULL REFERENCES inventory.company_skus(id),
    batch_id        INT REFERENCES inventory.batches(id),
    quantity        DECIMAL(10,2) NOT NULL DEFAULT 0,
    is_active       BOOLEAN DEFAULT TRUE,
    last_updated    TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (warehouse_id, sku_id, batch_id)
);

CREATE TABLE inventory.movements (
    id                      SERIAL PRIMARY KEY,
    company_id              INT NOT NULL REFERENCES shared.companies(id),
    warehouse_id            INT NOT NULL REFERENCES shared.warehouses(id),
    target_warehouse_id     INT REFERENCES shared.warehouses(id),
    status_id               INT NOT NULL REFERENCES inventory.movement_statuses(id),
    type_id                 INT NOT NULL REFERENCES inventory.movement_types(id),
    movement_date           TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    notes                   TEXT,
    is_active               BOOLEAN DEFAULT TRUE,
    created_at              TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE inventory.movement_details (
    id              SERIAL PRIMARY KEY,
    movement_id     INT NOT NULL REFERENCES inventory.movements(id),
    sku_id          INT NOT NULL REFERENCES inventory.company_skus(id),
    batch_id        INT REFERENCES inventory.batches(id),
    quantity        DECIMAL(10,2) NOT NULL,
    unit_cost       DECIMAL(10,2),
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE inventory.kardex (
    id                      SERIAL PRIMARY KEY,
    company_id              INT NOT NULL REFERENCES shared.companies(id),
    warehouse_id            INT NOT NULL REFERENCES shared.warehouses(id),
    sku_id                  INT NOT NULL REFERENCES inventory.company_skus(id),
    batch_id                INT REFERENCES inventory.batches(id),
    movement_detail_id      INT NOT NULL REFERENCES inventory.movement_details(id),
    type_id                 INT NOT NULL REFERENCES inventory.movement_types(id),
    quantity                DECIMAL(10,2) NOT NULL,
    balance_after           DECIMAL(10,2) NOT NULL,
    is_active               BOOLEAN DEFAULT TRUE,
    created_at              TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ==========================================
-- SCHEMA: sales
-- ==========================================

CREATE TABLE sales.sale_statuses (
    id          SERIAL PRIMARY KEY,
    code        VARCHAR(20) NOT NULL UNIQUE,
    name        VARCHAR(50) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.customers (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    name        VARCHAR(150) NOT NULL,
    phone       VARCHAR(20),
    email       VARCHAR(100),
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.sellers (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    name        VARCHAR(150) NOT NULL,
    phone       VARCHAR(20),
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.sales (
    id              SERIAL PRIMARY KEY,
    company_id      INT NOT NULL REFERENCES shared.companies(id),
    warehouse_id    INT NOT NULL REFERENCES shared.warehouses(id),
    seller_id       INT REFERENCES sales.sellers(id),
    customer_id     INT REFERENCES sales.customers(id),
    status_id       INT NOT NULL REFERENCES sales.sale_statuses(id),
    sale_date       TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    notes           TEXT,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.sale_details (
    id              SERIAL PRIMARY KEY,
    sale_id         INT NOT NULL REFERENCES sales.sales(id),
    sku_id          INT NOT NULL REFERENCES inventory.company_skus(id),
    batch_id        INT REFERENCES inventory.batches(id),
    quantity        DECIMAL(10,2) NOT NULL,
    unit_price      DECIMAL(10,2) NOT NULL,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.receipts (
    id              SERIAL PRIMARY KEY,
    sale_id         INT NOT NULL REFERENCES sales.sales(id),
    total_amount    DECIMAL(10,2) NOT NULL,
    issued_at       TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- PdV / POS

CREATE TABLE sales.pdv_stations (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    name        VARCHAR(100) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_waiters (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    name        VARCHAR(150) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_tables (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    name        VARCHAR(50) NOT NULL,
    capacity    INT,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_menus (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    name        VARCHAR(100) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_menu_items (
    id          SERIAL PRIMARY KEY,
    menu_id     INT NOT NULL REFERENCES sales.pdv_menus(id),
    sku_id      INT NOT NULL REFERENCES inventory.company_skus(id),
    station_id  INT REFERENCES sales.pdv_stations(id),
    name        VARCHAR(150) NOT NULL,
    price       DECIMAL(10,2) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_order_statuses (
    id          SERIAL PRIMARY KEY,
    code        VARCHAR(20) NOT NULL UNIQUE,
    name        VARCHAR(50) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_orders (
    id              SERIAL PRIMARY KEY,
    company_id      INT NOT NULL REFERENCES shared.companies(id),
    table_id        INT NOT NULL REFERENCES sales.pdv_tables(id),
    waiter_id       INT NOT NULL REFERENCES sales.pdv_waiters(id),
    status_id       INT NOT NULL REFERENCES sales.pdv_order_statuses(id),
    customer_id     INT REFERENCES sales.customers(id),
    sale_id         INT REFERENCES sales.sales(id),
    opened_at       TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    closed_at       TIMESTAMP,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_order_details (
    id              SERIAL PRIMARY KEY,
    order_id        INT NOT NULL REFERENCES sales.pdv_orders(id),
    menu_item_id    INT NOT NULL REFERENCES sales.pdv_menu_items(id),
    quantity        DECIMAL(10,2) NOT NULL,
    unit_price      DECIMAL(10,2) NOT NULL,
    notes           TEXT,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_comanda_statuses (
    id          SERIAL PRIMARY KEY,
    code        VARCHAR(20) NOT NULL UNIQUE,
    name        VARCHAR(50) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_comandas (
    id          SERIAL PRIMARY KEY,
    order_id    INT NOT NULL REFERENCES sales.pdv_orders(id),
    station_id  INT NOT NULL REFERENCES sales.pdv_stations(id),
    status_id   INT NOT NULL REFERENCES sales.pdv_comanda_statuses(id),
    sent_at     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_comanda_details (
    id              SERIAL PRIMARY KEY,
    comanda_id      INT NOT NULL REFERENCES sales.pdv_comandas(id),
    order_detail_id INT NOT NULL REFERENCES sales.pdv_order_details(id),
    quantity        DECIMAL(10,2) NOT NULL,
    notes           TEXT,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
-- ==========================================
-- SEED DATA
-- ==========================================

INSERT INTO inventory.movement_statuses (code, name) VALUES
('DRAFT',       'Borrador'),
('CONFIRMED',   'Confirmado'),
('CANCELLED',   'Anulado');

INSERT INTO inventory.movement_types (code, name, operation) VALUES
('IN_PURCHASE',     'Entrada por Compra',       '+'),
('OUT_SALE',        'Salida por Venta',          '-'),
('ADJ_ADD',         'Ajuste Positivo',           '+'),
('ADJ_SUB',         'Ajuste Negativo',           '-'),
('TRANSFER_OUT',    'Traspaso Salida',           '-'),
('TRANSFER_IN',     'Traspaso Entrada',          '+');

INSERT INTO sales.sale_statuses (code, name) VALUES
('DRAFT',       'Borrador'),
('CONFIRMED',   'Confirmada'),
('CANCELLED',   'Anulada'),
('RETURNED',    'Devuelta');

INSERT INTO sales.pdv_order_statuses (code, name) VALUES
('OPEN',        'Abierta'),
('BILLED',      'Facturada'),
('PAID',        'Pagada'),
('CANCELLED',   'Anulada');

-- ==========================================
-- SEQUENCE SYNC
-- ==========================================

SELECT setval(pg_get_serial_sequence('inventory.movement_statuses', 'id'), coalesce(max(id), 0) + 1, false) FROM inventory.movement_statuses;
SELECT setval(pg_get_serial_sequence('inventory.movement_types', 'id'), coalesce(max(id), 0) + 1, false) FROM inventory.movement_types;
SELECT setval(pg_get_serial_sequence('sales.sale_statuses', 'id'), coalesce(max(id), 0) + 1, false) FROM sales.sale_statuses;
SELECT setval(pg_get_serial_sequence('sales.pdv_order_statuses', 'id'), coalesce(max(id), 0) + 1, false) FROM sales.pdv_order_statuses;

INSERT INTO sales.pdv_comanda_statuses (code, name) VALUES
('SENT',        'Enviada'),
('IN_PROGRESS', 'En preparación'),
('READY',       'Lista'),
('DELIVERED',   'Entregada'),
('CANCELLED',   'Anulada');

SELECT setval(pg_get_serial_sequence('sales.pdv_comanda_statuses', 'id'), coalesce(max(id), 0) + 1, false) FROM sales.pdv_comanda_statuses;