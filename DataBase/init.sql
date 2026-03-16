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
    id                  SERIAL PRIMARY KEY,
    company_product_id  INT NOT NULL REFERENCES inventory.company_products(id),
    internal_sku        VARCHAR(50) NOT NULL,
    retail_price        DECIMAL(10,2) DEFAULT 0.00,
    is_active           BOOLEAN DEFAULT TRUE,
    created_at          TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
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
    reserved_quantity   DECIMAL(10,2) NOT NULL DEFAULT 0,
    is_active       BOOLEAN DEFAULT TRUE,
    last_updated    TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (warehouse_id, sku_id, batch_id)
);

CREATE TABLE inventory.movements (
    id                  SERIAL PRIMARY KEY,
    company_id          INT NOT NULL REFERENCES shared.companies(id),
    warehouse_id        INT NOT NULL REFERENCES shared.warehouses(id),
    target_warehouse_id INT REFERENCES shared.warehouses(id),
    status_id           INT NOT NULL REFERENCES inventory.movement_statuses(id),
    type_id             INT NOT NULL REFERENCES inventory.movement_types(id),
    movement_date       TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    notes               TEXT,
    is_active           BOOLEAN DEFAULT TRUE,
    created_at          TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE inventory.movement_details (
    id          SERIAL PRIMARY KEY,
    movement_id INT NOT NULL REFERENCES inventory.movements(id),
    sku_id      INT NOT NULL REFERENCES inventory.company_skus(id),
    batch_id    INT REFERENCES inventory.batches(id),
    quantity    DECIMAL(10,2) NOT NULL,
    unit_cost   DECIMAL(10,2),
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE inventory.kardex (
    id                  SERIAL PRIMARY KEY,
    company_id          INT NOT NULL REFERENCES shared.companies(id),
    warehouse_id        INT NOT NULL REFERENCES shared.warehouses(id),
    sku_id              INT NOT NULL REFERENCES inventory.company_skus(id),
    batch_id            INT REFERENCES inventory.batches(id),
    movement_detail_id  INT NOT NULL REFERENCES inventory.movement_details(id),
    type_id             INT NOT NULL REFERENCES inventory.movement_types(id),
    quantity            DECIMAL(10,2) NOT NULL,
    balance_after       DECIMAL(10,2) NOT NULL,
    is_active           BOOLEAN DEFAULT TRUE,
    created_at          TIMESTAMP DEFAULT CURRENT_TIMESTAMP
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
    id          SERIAL PRIMARY KEY,
    sale_id     INT NOT NULL REFERENCES sales.sales(id),
    sku_id      INT NOT NULL REFERENCES inventory.company_skus(id),
    batch_id    INT REFERENCES inventory.batches(id),
    quantity    DECIMAL(10,2) NOT NULL,
    unit_price  DECIMAL(10,2) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.receipts (
    id              SERIAL PRIMARY KEY,
    sale_id         INT NOT NULL REFERENCES sales.sales(id),
    total_amount    DECIMAL(10,2) NOT NULL,
    issued_at       TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active       BOOLEAN DEFAULT TRUE,
    created_at      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ==========================================
-- PdV / POS
-- ==========================================

CREATE TABLE sales.pdv_item_statuses (
    id          SERIAL PRIMARY KEY,
    code        VARCHAR(20) NOT NULL UNIQUE,
    name        VARCHAR(50) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_stations (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    name        VARCHAR(100) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_station_categories (
    id                  SERIAL PRIMARY KEY,
    station_id          INT NOT NULL REFERENCES sales.pdv_stations(id),
    global_category_id  INT NOT NULL REFERENCES shared.global_categories(id),
    is_active           BOOLEAN DEFAULT TRUE,
    created_at          TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (station_id, global_category_id)
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
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (menu_id, sku_id)
);

CREATE TABLE sales.pdv_order_statuses (
    id          SERIAL PRIMARY KEY,
    code        VARCHAR(20) NOT NULL UNIQUE,
    name        VARCHAR(50) NOT NULL,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_orders (
    id          SERIAL PRIMARY KEY,
    company_id  INT NOT NULL REFERENCES shared.companies(id),
    table_id    INT NOT NULL REFERENCES sales.pdv_tables(id),
    waiter_id   INT NOT NULL REFERENCES sales.pdv_waiters(id),
    status_id   INT NOT NULL REFERENCES sales.pdv_order_statuses(id),
    customer_id INT REFERENCES sales.customers(id),
    sale_id     INT REFERENCES sales.sales(id),
    opened_at   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    closed_at   TIMESTAMP,
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sales.pdv_order_details (
    id              SERIAL PRIMARY KEY,
    order_id        INT NOT NULL REFERENCES sales.pdv_orders(id),
    menu_item_id    INT NOT NULL REFERENCES sales.pdv_menu_items(id),
    station_id      INT REFERENCES sales.pdv_stations(id),
    status_id       INT NOT NULL REFERENCES sales.pdv_item_statuses(id),
    quantity        DECIMAL(10,2) NOT NULL,
    unit_price      DECIMAL(10,2) NOT NULL,
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
('IN_PURCHASE',  'Entrada por Compra',  '+'),
('OUT_SALE',     'Salida por Venta',    '-'),
('ADJ_ADD',      'Ajuste Positivo',     '+'),
('ADJ_SUB',      'Ajuste Negativo',     '-'),
('TRANSFER_OUT', 'Traspaso Salida',     '-'),
('TRANSFER_IN',  'Traspaso Entrada',    '+');

INSERT INTO sales.sale_statuses (code, name) VALUES
('DRAFT',     'Borrador'),
('CONFIRMED', 'Confirmada'),
('CANCELLED', 'Anulada'),
('RETURNED',  'Devuelta');

INSERT INTO sales.pdv_order_statuses (code, name) VALUES
('OPEN',      'Abierta'),
('BILLED',    'Facturada'),
('PAID',      'Pagada'),
('CANCELLED', 'Anulada');

INSERT INTO sales.pdv_item_statuses (code, name) VALUES
('SENT',        'Enviado'),
('IN_PROGRESS', 'En preparación'),
('READY',       'Listo'),
('DELIVERED',   'Entregado'),
('CANCELLED',   'Anulado');

-- ==========================================
-- SEQUENCE SYNC
-- ==========================================

SELECT setval(pg_get_serial_sequence('inventory.movement_statuses', 'id'), coalesce(max(id), 0) + 1, false) FROM inventory.movement_statuses;
SELECT setval(pg_get_serial_sequence('inventory.movement_types', 'id'), coalesce(max(id), 0) + 1, false) FROM inventory.movement_types;
SELECT setval(pg_get_serial_sequence('sales.sale_statuses', 'id'), coalesce(max(id), 0) + 1, false) FROM sales.sale_statuses;
SELECT setval(pg_get_serial_sequence('sales.pdv_order_statuses', 'id'), coalesce(max(id), 0) + 1, false) FROM sales.pdv_order_statuses;
SELECT setval(pg_get_serial_sequence('sales.pdv_item_statuses', 'id'), coalesce(max(id), 0) + 1, false) FROM sales.pdv_item_statuses;

-- ==========================================
-- SEED DATA — Companies
-- ==========================================

INSERT INTO shared.companies (name) VALUES
('Zapatería StepUp'),
('TechZone Electrónicos'),
('Restaurante El Fogón'),
('Abarrotes Don Carlos');

-- ==========================================
-- WAREHOUSES
-- ==========================================

INSERT INTO shared.warehouses (company_id, name) VALUES
(1, 'Sucursal Centro'),
(1, 'Sucursal Norte'),
(1, 'Sucursal Sur'),
(2, 'Almacén Principal'),
(2, 'Almacén Showroom'),
(2, 'Almacén Depósito'),
(3, 'Sucursal Centro'),
(3, 'Sucursal Norte'),
(3, 'Sucursal Sur'),
(4, 'Almacén Central'),
(4, 'Almacén Perecederos'),
(4, 'Almacén Seco');

-- ==========================================
-- GLOBAL CATEGORIES
-- ==========================================

INSERT INTO shared.global_categories (name) VALUES
('Calzado Deportivo'),
('Calzado Formal'),
('Calzado Casual'),
('Accesorios de Calzado'),
('Televisores'),
('Computadoras'),
('Teléfonos'),
('Accesorios Tecnológicos'),
('Carnes y Aves'),
('Mariscos'),
('Vegetales y Frutas'),
('Bebidas Alcohólicas'),
('Bebidas No Alcohólicas'),
('Lácteos'),
('Granos y Cereales'),
('Condimentos y Aceites'),
('Snacks y Dulces'),
('Productos de Limpieza'),
('Platos Preparados'),
('Cócteles y Tragos');

-- ==========================================
-- GLOBAL PRODUCTS
-- ==========================================

INSERT INTO shared.global_products (category_id, name, brand, upc_barcode) VALUES
(1, 'Zapatilla Running Pro', 'Nike', '7501010001001'),
(1, 'Zapatilla Training X', 'Adidas', '7501010001002'),
(1, 'Zapatilla Basketball Elite', 'Jordan', '7501010001003'),
(2, 'Zapato Oxford Clásico', 'Clarks', '7501010002001'),
(2, 'Zapato Derby Premium', 'Geox', '7501010002002'),
(3, 'Sandalia Urban', 'Crocs', '7501010003001'),
(3, 'Mocasín Casual', 'Timberland', '7501010003002'),
(4, 'Plantilla Ortopédica', 'Scholl', '7501010004001'),
(4, 'Cordones Premium', 'Genérico', '7501010004002'),
(5, 'Smart TV 55 pulgadas 4K', 'Samsung', '7501010005001'),
(5, 'Smart TV 43 pulgadas FHD', 'LG', '7501010005002'),
(5, 'Smart TV 65 pulgadas OLED', 'Sony', '7501010005003'),
(6, 'Laptop 15 Core i5', 'HP', '7501010006001'),
(6, 'Laptop 14 Core i7', 'Dell', '7501010006002'),
(6, 'Desktop Gaming', 'Asus', '7501010006003'),
(7, 'Smartphone 128GB', 'Samsung', '7501010007001'),
(7, 'Smartphone 256GB Pro', 'iPhone', '7501010007002'),
(8, 'Mouse Inalámbrico', 'Logitech', '7501010008001'),
(8, 'Teclado Mecánico', 'Redragon', '7501010008002'),
(8, 'Audífonos Bluetooth', 'Sony', '7501010008003'),
(9, 'Pollo Entero', 'Avícola del Sur', '7501010009001'),
(9, 'Carne Molida', 'Carnes Premium', '7501010009002'),
(9, 'Costillas de Cerdo', 'Carnes Premium', '7501010009003'),
(9, 'Filete de Res', 'Carnes Premium', '7501010009004'),
(10, 'Filete de Pescado', 'Mar Fresco', '7501010010001'),
(10, 'Camarones XL', 'Mar Fresco', '7501010010002'),
(11, 'Tomate', 'Huerto Verde', '7501010011001'),
(11, 'Lechuga', 'Huerto Verde', '7501010011002'),
(11, 'Cebolla', 'Huerto Verde', '7501010011003'),
(11, 'Papa', 'Huerto Verde', '7501010011004'),
(12, 'Cerveza Pilsener 330ml', 'Pilsener', '7501010012001'),
(12, 'Vino Tinto 750ml', 'Santa Helena', '7501010012002'),
(12, 'Ron 750ml', 'Cacique', '7501010012003'),
(12, 'Whisky 750ml', 'Black Label', '7501010012004'),
(13, 'Coca Cola 500ml', 'Coca Cola', '7501010013001'),
(13, 'Agua Mineral 600ml', 'Crystal', '7501010013002'),
(13, 'Jugo Natural 1L', 'Del Valle', '7501010013003'),
(14, 'Queso Mozzarella kg', 'Lácteos del Norte', '7501010014001'),
(14, 'Mantequilla 200g', 'Lácteos del Norte', '7501010014002'),
(15, 'Arroz 1kg', 'Granos del Sur', '7501010015001'),
(15, 'Harina de Trigo 1kg', 'Selecta', '7501010015002'),
(16, 'Aceite Vegetal 1L', 'Palma de Oro', '7501010016001'),
(16, 'Sal 1kg', 'Refisal', '7501010016002'),
(19, 'Hamburguesa Clásica', NULL, '7501010019001'),
(19, 'Lomito Completo', NULL, '7501010019002'),
(19, 'Pollo a la Parrilla', NULL, '7501010019003'),
(19, 'Filete de Pescado Frito', NULL, '7501010019004'),
(19, 'Papas Fritas Porción', NULL, '7501010019005'),
(20, 'Mojito Clásico', NULL, '7501010020001'),
(20, 'Piña Colada', NULL, '7501010020002'),
(20, 'Margarita', NULL, '7501010020003'),
(15, 'Fideo 400g', 'Lucchetti', '7501010015003'),
(17, 'Galletas Oreo 200g', 'Nabisco', '7501010017001'),
(17, 'Chocolate 100g', 'Nestlé', '7501010017002'),
(18, 'Detergente 1kg', 'Ariel', '7501010018001'),
(18, 'Desinfectante 1L', 'Lysol', '7501010018002'),
(13, 'Sprite 500ml', 'Coca Cola', '7501010013004'),
(12, 'Cerveza Corona 330ml', 'Corona', '7501010012005');

-- ==========================================
-- COMPANY 1 — Zapatería StepUp
-- ==========================================

INSERT INTO inventory.company_attributes (company_id, name) VALUES
(1, 'Talla'),
(1, 'Color');

INSERT INTO inventory.company_products (company_id, global_product_id, local_name_alias, wholesale_price) VALUES
(1, 1, 'Nike Running Pro', 45.00),
(1, 2, 'Adidas Training X', 40.00),
(1, 3, 'Jordan Basketball Elite', 85.00),
(1, 4, 'Oxford Clásico Clarks', 55.00),
(1, 5, 'Derby Geox Premium', 60.00),
(1, 6, 'Crocs Urban', 25.00),
(1, 7, 'Timberland Mocasín', 50.00),
(1, 8, 'Plantilla Scholl', 8.00),
(1, 9, 'Cordones Premium', 2.00);

INSERT INTO inventory.company_skus (company_product_id, internal_sku, retail_price) VALUES
(1, 'STEP-NIKE-RUN-38', 75.00),
(1, 'STEP-NIKE-RUN-39', 75.00),
(1, 'STEP-NIKE-RUN-40', 75.00),
(1, 'STEP-NIKE-RUN-41', 75.00),
(1, 'STEP-NIKE-RUN-42', 75.00),
(2, 'STEP-ADID-TRX-38', 65.00),
(2, 'STEP-ADID-TRX-39', 65.00),
(2, 'STEP-ADID-TRX-40', 65.00),
(2, 'STEP-ADID-TRX-41', 65.00),
(3, 'STEP-JORD-BAS-40', 140.00),
(3, 'STEP-JORD-BAS-41', 140.00),
(3, 'STEP-JORD-BAS-42', 140.00),
(4, 'STEP-OXFO-CLA-39', 90.00),
(4, 'STEP-OXFO-CLA-40', 90.00),
(4, 'STEP-OXFO-CLA-41', 90.00),
(5, 'STEP-DERB-GEX-39', 95.00),
(5, 'STEP-DERB-GEX-40', 95.00),
(5, 'STEP-DERB-GEX-41', 95.00),
(6, 'STEP-CROC-URB-38', 40.00),
(6, 'STEP-CROC-URB-40', 40.00),
(6, 'STEP-CROC-URB-42', 40.00),
(8, 'STEP-PLAN-SCH-UND', 15.00),
(9, 'STEP-CORD-PRE-UND', 4.00);

INSERT INTO inventory.sku_attribute_values (sku_id, attribute_id, value) VALUES
(1, 1, '38'),(1, 2, 'Negro'),
(2, 1, '39'),(2, 2, 'Negro'),
(3, 1, '40'),(3, 2, 'Negro'),
(4, 1, '41'),(4, 2, 'Blanco'),
(5, 1, '42'),(5, 2, 'Blanco'),
(6, 1, '38'),(6, 2, 'Azul'),
(7, 1, '39'),(7, 2, 'Azul'),
(8, 1, '40'),(8, 2, 'Negro'),
(9, 1, '41'),(9, 2, 'Rojo'),
(10, 1, '40'),(10, 2, 'Negro'),
(11, 1, '41'),(11, 2, 'Blanco'),
(12, 1, '42'),(12, 2, 'Rojo'),
(13, 1, '39'),(13, 2, 'Café'),
(14, 1, '40'),(14, 2, 'Café'),
(15, 1, '41'),(15, 2, 'Negro'),
(16, 1, '39'),(16, 2, 'Café'),
(17, 1, '40'),(17, 2, 'Negro'),
(18, 1, '41'),(18, 2, 'Marrón'),
(19, 1, '38'),(19, 2, 'Negro'),
(20, 1, '40'),(20, 2, 'Azul'),
(21, 1, '42'),(21, 2, 'Rojo');

INSERT INTO inventory.stocks (warehouse_id, sku_id, quantity, reserved_quantity) VALUES
(1, 1, 10, 0),(1, 2, 10, 0),(1, 3, 8, 0),(1, 4, 6, 0),(1, 5, 6, 0),
(1, 6, 8, 0),(1, 7, 8, 0),(1, 8, 6, 0),(1, 9, 4, 0),
(1, 10, 5, 0),(1, 11, 5, 0),(1, 12, 4, 0),
(1, 13, 6, 0),(1, 14, 6, 0),(1, 15, 5, 0),
(1, 22, 20, 0),(1, 23, 15, 0),
(2, 1, 8, 0),(2, 2, 8, 0),(2, 3, 6, 0),
(2, 6, 6, 0),(2, 7, 6, 0),(2, 8, 5, 0),
(2, 16, 6, 0),(2, 17, 6, 0),(2, 18, 5, 0),
(2, 22, 15, 0),(2, 23, 10, 0),
(3, 4, 5, 0),(3, 5, 5, 0),
(3, 10, 4, 0),(3, 11, 4, 0),(3, 12, 3, 0),
(3, 19, 8, 0),(3, 20, 8, 0),(3, 21, 6, 0),
(3, 22, 12, 0),(3, 23, 10, 0);

INSERT INTO inventory.movements (company_id, warehouse_id, status_id, type_id, notes) VALUES
(1, 1, 2, 1, 'Compra inicial Sucursal Centro'),
(1, 2, 2, 1, 'Compra inicial Sucursal Norte'),
(1, 3, 2, 1, 'Compra inicial Sucursal Sur');

INSERT INTO inventory.movement_details (movement_id, sku_id, quantity, unit_cost) VALUES
(1, 1, 10, 45.00),(1, 2, 10, 45.00),(1, 3, 8, 45.00),(1, 4, 6, 45.00),(1, 5, 6, 45.00),
(1, 6, 8, 40.00),(1, 7, 8, 40.00),(1, 8, 6, 40.00),(1, 9, 4, 40.00),
(1, 10, 5, 85.00),(1, 11, 5, 85.00),(1, 12, 4, 85.00),
(1, 13, 6, 55.00),(1, 14, 6, 55.00),(1, 15, 5, 55.00),
(1, 22, 20, 8.00),(1, 23, 15, 2.00),
(2, 1, 8, 45.00),(2, 2, 8, 45.00),(2, 3, 6, 45.00),
(2, 6, 6, 40.00),(2, 7, 6, 40.00),(2, 8, 5, 40.00),
(2, 16, 6, 60.00),(2, 17, 6, 60.00),(2, 18, 5, 60.00),
(2, 22, 15, 8.00),(2, 23, 10, 2.00),
(3, 4, 5, 55.00),(3, 5, 5, 55.00),
(3, 10, 4, 85.00),(3, 11, 4, 85.00),(3, 12, 3, 85.00),
(3, 19, 8, 25.00),(3, 20, 8, 25.00),(3, 21, 6, 25.00),
(3, 22, 12, 8.00),(3, 23, 10, 2.00);

INSERT INTO inventory.kardex (company_id, warehouse_id, sku_id, movement_detail_id, type_id, quantity, balance_after) VALUES
(1, 1, 1, 1, 1, 10, 10),(1, 1, 2, 2, 1, 10, 10),(1, 1, 3, 3, 1, 8, 8),
(1, 1, 4, 4, 1, 6, 6),(1, 1, 5, 5, 1, 6, 6),(1, 1, 6, 6, 1, 8, 8),
(1, 1, 7, 7, 1, 8, 8),(1, 1, 8, 8, 1, 6, 6),(1, 1, 9, 9, 1, 4, 4),
(1, 1, 10, 10, 1, 5, 5),(1, 1, 11, 11, 1, 5, 5),(1, 1, 12, 12, 1, 4, 4),
(1, 1, 13, 13, 1, 6, 6),(1, 1, 14, 14, 1, 6, 6),(1, 1, 15, 15, 1, 5, 5),
(1, 1, 22, 16, 1, 20, 20),(1, 1, 23, 17, 1, 15, 15),
(1, 2, 1, 18, 1, 8, 8),(1, 2, 2, 19, 1, 8, 8),(1, 2, 3, 20, 1, 6, 6),
(1, 2, 6, 21, 1, 6, 6),(1, 2, 7, 22, 1, 6, 6),(1, 2, 8, 23, 1, 5, 5),
(1, 2, 16, 24, 1, 6, 6),(1, 2, 17, 25, 1, 6, 6),(1, 2, 18, 26, 1, 5, 5),
(1, 2, 22, 27, 1, 15, 15),(1, 2, 23, 28, 1, 10, 10),
(1, 3, 4, 29, 1, 5, 5),(1, 3, 5, 30, 1, 5, 5),
(1, 3, 10, 31, 1, 4, 4),(1, 3, 11, 32, 1, 4, 4),(1, 3, 12, 33, 1, 3, 3),
(1, 3, 19, 34, 1, 8, 8),(1, 3, 20, 35, 1, 8, 8),(1, 3, 21, 36, 1, 6, 6),
(1, 3, 22, 37, 1, 12, 12),(1, 3, 23, 38, 1, 10, 10);

INSERT INTO sales.pdv_stations (company_id, name) VALUES
(1, 'Caja Sucursal Centro'),
(1, 'Caja Sucursal Norte'),
(1, 'Caja Sucursal Sur');

INSERT INTO sales.pdv_station_categories (station_id, global_category_id) VALUES
(1, 1),(1, 2),(1, 3),(1, 4),
(2, 1),(2, 2),(2, 3),(2, 4),
(3, 1),(3, 2),(3, 3),(3, 4);

INSERT INTO sales.pdv_waiters (company_id, name) VALUES
(1, 'Vendedor Centro 1'),
(1, 'Vendedor Centro 2'),
(1, 'Vendedor Norte 1'),
(1, 'Vendedor Sur 1');

INSERT INTO sales.pdv_tables (company_id, name, capacity) VALUES
(1, 'Punto Venta Centro', 1),
(1, 'Punto Venta Norte', 1),
(1, 'Punto Venta Sur', 1);

INSERT INTO sales.pdv_menus (company_id, name) VALUES
(1, 'Catálogo Deportivo'),
(1, 'Catálogo Formal'),
(1, 'Catálogo Casual');

INSERT INTO sales.pdv_menu_items (menu_id, sku_id, station_id) VALUES
(1, 1, 1),(1, 2, 1),(1, 3, 1),(1, 4, 1),(1, 5, 1),
(1, 6, 1),(1, 7, 1),(1, 8, 1),(1, 9, 1),
(2, 13, 1),(2, 14, 1),(2, 15, 1),
(2, 16, 2),(2, 17, 2),(2, 18, 2),
(3, 19, 3),(3, 20, 3),(3, 21, 3),
(3, 10, 1),(3, 11, 1),(3, 12, 1),
(3, 22, 1),(3, 23, 1);

INSERT INTO sales.customers (company_id, name, phone, email) VALUES
(1, 'Carlos Mendoza', '77801001', 'carlos@email.com'),
(1, 'Ana García', '77801002', 'ana@email.com'),
(1, 'Luis Pérez', '77801003', NULL);

INSERT INTO sales.sellers (company_id, name, phone) VALUES
(1, 'Roberto Díaz', '77811001'),
(1, 'Sofía Morales', '77811002');

INSERT INTO sales.sales (company_id, warehouse_id, seller_id, customer_id, status_id, notes) VALUES
(1, 1, 1, 1, 2, 'Venta zapatillas Nike');

INSERT INTO sales.sale_details (sale_id, sku_id, quantity, unit_price) VALUES
(1, 3, 1, 75.00),
(1, 23, 1, 4.00);

INSERT INTO sales.receipts (sale_id, total_amount) VALUES
(1, 79.00);

INSERT INTO sales.pdv_orders (company_id, table_id, waiter_id, status_id, customer_id, sale_id, opened_at, closed_at) VALUES
(1, 1, 1, 2, 1, 1, CURRENT_TIMESTAMP - INTERVAL '30 minutes', CURRENT_TIMESTAMP);

INSERT INTO sales.pdv_order_details (order_id, menu_item_id, station_id, status_id, quantity, unit_price) VALUES
(1, 3, 1, 4, 1, 75.00),
(1, 22, 1, 4, 1, 4.00);

-- ==========================================
-- COMPANY 2 — TechZone Electrónicos
-- Solo ventas directas, sin PdV
-- ==========================================

INSERT INTO inventory.company_attributes (company_id, name) VALUES
(2, 'Color'),
(2, 'Almacenamiento');

INSERT INTO inventory.company_products (company_id, global_product_id, local_name_alias, wholesale_price) VALUES
(2, 10, 'Smart TV 55" Samsung 4K', 450.00),
(2, 11, 'Smart TV 43" LG FHD', 280.00),
(2, 12, 'Smart TV 65" Sony OLED', 850.00),
(2, 13, 'Laptop HP Core i5', 500.00),
(2, 14, 'Laptop Dell Core i7', 750.00),
(2, 16, 'Samsung Galaxy 128GB', 350.00),
(2, 17, 'iPhone Pro 256GB', 900.00),
(2, 18, 'Mouse Logitech', 20.00),
(2, 19, 'Teclado Redragon', 35.00),
(2, 20, 'Audífonos Sony BT', 60.00);

INSERT INTO inventory.company_skus (company_product_id, internal_sku, retail_price) VALUES
(10, 'TECH-TV55-SAM-BLK', 650.00),
(11, 'TECH-TV43-LG-BLK', 420.00),
(12, 'TECH-TV65-SON-BLK', 1200.00),
(13, 'TECH-LAP-HP-256', 750.00),
(14, 'TECH-LAP-DEL-512', 1100.00),
(15, 'TECH-SAM-128-BLK', 550.00),
(15, 'TECH-SAM-128-WHT', 550.00),
(16, 'TECH-IPH-256-BLK', 1300.00),
(16, 'TECH-IPH-256-WHT', 1300.00),
(17, 'TECH-MOU-LOG-BLK', 35.00),
(18, 'TECH-TEC-RED-BLK', 55.00),
(19, 'TECH-AUD-SON-BLK', 90.00);

INSERT INTO inventory.sku_attribute_values (sku_id, attribute_id, value) VALUES
(24, 3, 'Negro'),(25, 3, 'Negro'),(26, 3, 'Negro'),
(27, 4, '256GB'),(28, 4, '512GB'),
(29, 3, 'Negro'),(29, 4, '128GB'),
(30, 3, 'Blanco'),(30, 4, '128GB'),
(31, 3, 'Negro'),(31, 4, '256GB'),
(32, 3, 'Blanco'),(32, 4, '256GB'),
(33, 3, 'Negro'),(34, 3, 'Negro'),(35, 3, 'Negro');

INSERT INTO inventory.stocks (warehouse_id, sku_id, quantity, reserved_quantity) VALUES
(4, 24, 15, 0),(4, 25, 20, 0),(4, 26, 5, 0),
(4, 27, 10, 0),(4, 28, 8, 0),
(4, 29, 12, 0),(4, 30, 8, 0),
(4, 31, 6, 0),(4, 32, 6, 0),
(5, 24, 10, 0),(5, 25, 15, 0),
(5, 33, 30, 0),(5, 34, 25, 0),(5, 35, 20, 0),
(6, 24, 8, 0),(6, 25, 10, 0),(6, 26, 3, 0),
(6, 27, 5, 0),(6, 28, 4, 0);

INSERT INTO inventory.movements (company_id, warehouse_id, status_id, type_id, notes) VALUES
(2, 4, 2, 1, 'Compra inicial Almacén Principal'),
(2, 5, 2, 1, 'Compra inicial Showroom'),
(2, 6, 2, 1, 'Compra inicial Depósito');

INSERT INTO inventory.movement_details (movement_id, sku_id, quantity, unit_cost) VALUES
(4, 24, 15, 450.00),(4, 25, 20, 280.00),(4, 26, 5, 850.00),
(4, 27, 10, 500.00),(4, 28, 8, 750.00),
(4, 29, 12, 350.00),(4, 30, 8, 350.00),
(4, 31, 6, 900.00),(4, 32, 6, 900.00),
(5, 24, 10, 450.00),(5, 25, 15, 280.00),
(5, 33, 30, 20.00),(5, 34, 25, 35.00),(5, 35, 20, 60.00),
(6, 24, 8, 450.00),(6, 25, 10, 280.00),(6, 26, 3, 850.00),
(6, 27, 5, 500.00),(6, 28, 4, 750.00);

INSERT INTO inventory.kardex (company_id, warehouse_id, sku_id, movement_detail_id, type_id, quantity, balance_after) VALUES
(2, 4, 24, 39, 1, 15, 15),(2, 4, 25, 40, 1, 20, 20),(2, 4, 26, 41, 1, 5, 5),
(2, 4, 27, 42, 1, 10, 10),(2, 4, 28, 43, 1, 8, 8),
(2, 4, 29, 44, 1, 12, 12),(2, 4, 30, 45, 1, 8, 8),
(2, 4, 31, 46, 1, 6, 6),(2, 4, 32, 47, 1, 6, 6),
(2, 5, 24, 48, 1, 10, 10),(2, 5, 25, 49, 1, 15, 15),
(2, 5, 33, 50, 1, 30, 30),(2, 5, 34, 51, 1, 25, 25),(2, 5, 35, 52, 1, 20, 20),
(2, 6, 24, 53, 1, 8, 8),(2, 6, 25, 54, 1, 10, 10),(2, 6, 26, 55, 1, 3, 3),
(2, 6, 27, 56, 1, 5, 5),(2, 6, 28, 57, 1, 4, 4);

INSERT INTO sales.customers (company_id, name, phone, email) VALUES
(2, 'Empresa ABC Ltda', '77802001', 'compras@abc.com'),
(2, 'Miguel Torres', '77802002', 'miguel@email.com'),
(2, 'Laura Vega', '77802003', NULL);

INSERT INTO sales.sellers (company_id, name, phone) VALUES
(2, 'Fernando Ruiz', '77812001'),
(2, 'Camila Ortiz', '77812002');

INSERT INTO sales.sales (company_id, warehouse_id, seller_id, customer_id, status_id, notes) VALUES
(2, 4, 3, 4, 2, 'Venta TV + Laptop empresa ABC'),
(2, 4, 4, 5, 2, 'Venta smartphone Miguel Torres');

INSERT INTO sales.sale_details (sale_id, sku_id, quantity, unit_price) VALUES
(2, 24, 1, 650.00),(2, 27, 1, 750.00),
(3, 31, 1, 1300.00);

INSERT INTO sales.receipts (sale_id, total_amount) VALUES
(2, 1400.00),
(3, 1300.00);

-- ==========================================
-- COMPANY 3 — Restaurante El Fogón
-- PdV con cocina + bar por sucursal
-- ==========================================

INSERT INTO inventory.company_attributes (company_id, name) VALUES
(3, 'Presentación'),
(3, 'Tamaño');

INSERT INTO inventory.company_products (company_id, global_product_id, local_name_alias, wholesale_price) VALUES
(3, 21, 'Pollo para cocina', 5.50),
(3, 22, 'Carne molida', 8.00),
(3, 23, 'Costillas cerdo', 10.00),
(3, 24, 'Filete de res', 12.00),
(3, 27, 'Tomate fresco', 0.80),
(3, 28, 'Lechuga', 0.50),
(3, 29, 'Cebolla', 0.40),
(3, 30, 'Papa', 0.60),
(3, 39, 'Arroz', 0.90),
(3, 43, 'Aceite vegetal', 2.50),
(3, 45, 'Hamburguesa Clásica', 4.00),
(3, 46, 'Lomito Completo', 6.00),
(3, 47, 'Pollo a la Parrilla', 5.00),
(3, 49, 'Papas Fritas Porción', 1.50),
(3, 31, 'Cerveza Pilsener', 1.20),
(3, 32, 'Vino Tinto', 8.00),
(3, 35, 'Coca Cola', 0.60),
(3, 36, 'Agua Mineral', 0.30),
(3, 51, 'Mojito Clásico', 3.00),
(3, 52, 'Piña Colada', 3.50);

INSERT INTO inventory.company_skus (company_product_id, internal_sku, retail_price) VALUES
(20, 'FOG-POLL-KG', 12.00),
(21, 'FOG-CARN-KG', 15.00),
(22, 'FOG-COST-KG', 18.00),
(23, 'FOG-FILE-KG', 22.00),
(24, 'FOG-TOMA-KG', 2.50),
(25, 'FOG-LECH-UND', 1.50),
(26, 'FOG-CEBO-KG', 1.20),
(27, 'FOG-PAPA-KG', 2.00),
(28, 'FOG-ARRO-KG', 2.50),
(29, 'FOG-ACEI-LT', 4.00),
(30, 'FOG-HAMB-CLA', 12.00),
(31, 'FOG-LOMI-COM', 15.00),
(32, 'FOG-POLL-PAR', 14.00),
(33, 'FOG-PAPA-POR', 5.00),
(34, 'FOG-CERV-330', 3.00),
(35, 'FOG-VINO-750', 18.00),
(36, 'FOG-COCA-500', 2.00),
(37, 'FOG-AGUA-600', 1.00),
(38, 'FOG-MOJI-CLA', 8.00),
(39, 'FOG-PINA-COL', 9.00);

INSERT INTO inventory.stocks (warehouse_id, sku_id, quantity, reserved_quantity) VALUES
(7, 36, 50, 0),(7, 37, 30, 0),(7, 38, 20, 0),(7, 39, 25, 0),
(7, 40, 40, 0),(7, 41, 35, 0),(7, 42, 50, 0),(7, 43, 30, 0),
(7, 44, 80, 0),(7, 45, 60, 0),
(7, 46, 100, 0),(7, 47, 80, 0),(7, 48, 60, 0),(7, 49, 40, 0),
(7, 50, 200, 0),(7, 51, 150, 0),(7, 52, 50, 0),(7, 53, 30, 0),(7, 54, 40, 0),(7, 55, 30, 0),
(8, 36, 40, 0),(8, 37, 25, 0),(8, 38, 15, 0),(8, 39, 20, 0),
(8, 40, 30, 0),(8, 41, 25, 0),(8, 42, 40, 0),(8, 43, 25, 0),
(8, 44, 60, 0),(8, 45, 50, 0),
(8, 46, 80, 0),(8, 47, 60, 0),(8, 50, 150, 0),(8, 51, 100, 0),(8, 52, 40, 0),(8, 53, 25, 0),
(9, 36, 35, 0),(9, 37, 20, 0),(9, 38, 12, 0),
(9, 40, 25, 0),(9, 41, 20, 0),(9, 42, 35, 0),
(9, 44, 50, 0),(9, 45, 40, 0),
(9, 46, 70, 0),(9, 47, 50, 0),(9, 50, 120, 0),(9, 51, 80, 0),(9, 54, 30, 0),(9, 55, 20, 0);

INSERT INTO inventory.movements (company_id, warehouse_id, status_id, type_id, notes) VALUES
(3, 7, 2, 1, 'Compra inicial Sucursal Centro'),
(3, 8, 2, 1, 'Compra inicial Sucursal Norte'),
(3, 9, 2, 1, 'Compra inicial Sucursal Sur');

INSERT INTO inventory.movement_details (movement_id, sku_id, quantity, unit_cost) VALUES
(7, 36, 50, 5.50),(7, 37, 30, 8.00),(7, 38, 20, 10.00),(7, 39, 25, 12.00),
(7, 40, 40, 0.80),(7, 41, 35, 0.50),(7, 42, 50, 0.40),(7, 43, 30, 0.60),
(7, 44, 80, 0.90),(7, 45, 60, 2.50),
(7, 46, 100, 4.00),(7, 47, 80, 6.00),(7, 48, 60, 5.00),(7, 49, 40, 1.50),
(7, 50, 200, 1.20),(7, 51, 150, 8.00),(7, 52, 50, 0.60),(7, 53, 30, 0.30),(7, 54, 40, 3.00),(7, 55, 30, 3.50),
(8, 36, 40, 5.50),(8, 37, 25, 8.00),(8, 38, 15, 10.00),(8, 39, 20, 12.00),
(8, 40, 30, 0.80),(8, 41, 25, 0.50),(8, 42, 40, 0.40),(8, 43, 25, 0.60),
(8, 44, 60, 0.90),(8, 45, 50, 2.50),
(8, 46, 80, 4.00),(8, 47, 60, 6.00),(8, 50, 150, 1.20),(8, 51, 100, 8.00),(8, 52, 40, 0.60),(8, 53, 25, 0.30),
(9, 36, 35, 5.50),(9, 37, 20, 8.00),(9, 38, 12, 10.00),
(9, 40, 25, 0.80),(9, 41, 20, 0.50),(9, 42, 35, 0.40),
(9, 44, 50, 0.90),(9, 45, 40, 2.50),
(9, 46, 70, 4.00),(9, 47, 50, 6.00),(9, 50, 120, 1.20),(9, 51, 80, 8.00),(9, 54, 30, 3.00),(9, 55, 20, 3.50);

INSERT INTO inventory.kardex (company_id, warehouse_id, sku_id, movement_detail_id, type_id, quantity, balance_after) VALUES
(3, 7, 36, 58, 1, 50, 50),(3, 7, 37, 59, 1, 30, 30),(3, 7, 38, 60, 1, 20, 20),(3, 7, 39, 61, 1, 25, 25),
(3, 7, 40, 62, 1, 40, 40),(3, 7, 41, 63, 1, 35, 35),(3, 7, 42, 64, 1, 50, 50),(3, 7, 43, 65, 1, 30, 30),
(3, 7, 44, 66, 1, 80, 80),(3, 7, 45, 67, 1, 60, 60),
(3, 7, 46, 68, 1, 100, 100),(3, 7, 47, 69, 1, 80, 80),(3, 7, 48, 70, 1, 60, 60),(3, 7, 49, 71, 1, 40, 40),
(3, 7, 50, 72, 1, 200, 200),(3, 7, 51, 73, 1, 150, 150),(3, 7, 52, 74, 1, 50, 50),(3, 7, 53, 75, 1, 30, 30),
(3, 7, 54, 76, 1, 40, 40),(3, 7, 55, 77, 1, 30, 30),
(3, 8, 36, 78, 1, 40, 40),(3, 8, 37, 79, 1, 25, 25),(3, 8, 38, 80, 1, 15, 15),(3, 8, 39, 81, 1, 20, 20),
(3, 8, 40, 82, 1, 30, 30),(3, 8, 41, 83, 1, 25, 25),(3, 8, 42, 84, 1, 40, 40),(3, 8, 43, 85, 1, 25, 25),
(3, 8, 44, 86, 1, 60, 60),(3, 8, 45, 87, 1, 50, 50),
(3, 8, 46, 88, 1, 80, 80),(3, 8, 47, 89, 1, 60, 60),(3, 8, 50, 90, 1, 150, 150),(3, 8, 51, 91, 1, 100, 100),
(3, 8, 52, 92, 1, 40, 40),(3, 8, 53, 93, 1, 25, 25),
(3, 9, 36, 94, 1, 35, 35),(3, 9, 37, 95, 1, 20, 20),(3, 9, 38, 96, 1, 12, 12),
(3, 9, 40, 97, 1, 25, 25),(3, 9, 41, 98, 1, 20, 20),(3, 9, 42, 99, 1, 35, 35),
(3, 9, 44, 100, 1, 50, 50),(3, 9, 45, 101, 1, 40, 40),
(3, 9, 46, 102, 1, 70, 70),(3, 9, 47, 103, 1, 50, 50),(3, 9, 50, 104, 1, 120, 120),(3, 9, 51, 105, 1, 80, 80),
(3, 9, 54, 106, 1, 30, 30),(3, 9, 55, 107, 1, 20, 20);

INSERT INTO sales.pdv_stations (company_id, name) VALUES
(3, 'Cocina Centro'),
(3, 'Bar Centro'),
(3, 'Cocina Norte'),
(3, 'Bar Norte'),
(3, 'Cocina Sur'),
(3, 'Bar Sur');

INSERT INTO sales.pdv_station_categories (station_id, global_category_id) VALUES
(4, 9),(4, 11),(4, 19),
(5, 12),(5, 13),(5, 20),
(6, 9),(6, 11),(6, 19),
(7, 12),(7, 13),(7, 20),
(8, 9),(8, 11),(8, 19),
(9, 12),(9, 13),(9, 20);

INSERT INTO sales.pdv_waiters (company_id, name) VALUES
(3, 'Carlos Mesero Centro'),
(3, 'Ana Mesera Centro'),
(3, 'Luis Mesero Norte'),
(3, 'Pedro Mesero Sur');

INSERT INTO sales.pdv_tables (company_id, name, capacity) VALUES
(3, 'Mesa C-1', 4),(3, 'Mesa C-2', 4),(3, 'Mesa C-3', 6),(3, 'Barra C-1', 2),
(3, 'Mesa N-1', 4),(3, 'Mesa N-2', 4),(3, 'Barra N-1', 2),
(3, 'Mesa S-1', 4),(3, 'Mesa S-2', 4),(3, 'Barra S-1', 2);

INSERT INTO sales.pdv_menus (company_id, name) VALUES
(3, 'Menú Cocina'),
(3, 'Menú Bar');

INSERT INTO sales.pdv_menu_items (menu_id, sku_id, station_id) VALUES
(4, 46, 4),(4, 47, 4),(4, 48, 4),(4, 49, 4),
(5, 50, 5),(5, 51, 5),(5, 52, 5),(5, 53, 5),(5, 54, 5),(5, 55, 5);

INSERT INTO sales.customers (company_id, name, phone, email) VALUES
(3, 'Juan Rodríguez', '77803001', 'juan@email.com'),
(3, 'María Flores', '77803002', NULL),
(3, 'Empresa Eventos SA', '77803003', 'eventos@empresa.com');

INSERT INTO sales.sellers (company_id, name, phone) VALUES
(3, 'Jefe Cocina Centro', '77813001'),
(3, 'Bartender Centro', '77813002');

INSERT INTO sales.sales (company_id, warehouse_id, seller_id, customer_id, status_id, notes) VALUES
(3, 7, 5, 7, 2, 'Mesa C-1 almuerzo'),
(3, 7, 6, 8, 2, 'Barra C-1 noche');

INSERT INTO sales.sale_details (sale_id, sku_id, quantity, unit_price) VALUES
(4, 46, 2, 12.00),(4, 47, 1, 15.00),(4, 52, 2, 2.00),
(5, 50, 4, 3.00),(5, 54, 2, 8.00);

INSERT INTO sales.receipts (sale_id, total_amount) VALUES
(4, 43.00),
(5, 28.00);

INSERT INTO sales.pdv_orders (company_id, table_id, waiter_id, status_id, customer_id, sale_id, opened_at, closed_at) VALUES
(3, 4, 5, 2, 7, 4, CURRENT_TIMESTAMP - INTERVAL '2 hours', CURRENT_TIMESTAMP),
(3, 7, 5, 2, 8, 5, CURRENT_TIMESTAMP - INTERVAL '1 hour', CURRENT_TIMESTAMP);

INSERT INTO sales.pdv_order_details (order_id, menu_item_id, station_id, status_id, quantity, unit_price) VALUES
(2, 26, 4, 4, 2, 12.00),
(2, 27, 4, 4, 1, 15.00),
(2, 30, 5, 4, 2, 2.00),
(3, 29, 5, 4, 4, 3.00),
(3, 34, 5, 4, 2, 8.00);

-- ==========================================
-- COMPANY 4 — Abarrotes Don Carlos
-- Solo ventas directas
-- ==========================================

INSERT INTO inventory.company_attributes (company_id, name) VALUES
(4, 'Presentación'),
(4, 'Peso');

INSERT INTO inventory.company_products (company_id, global_product_id, local_name_alias, wholesale_price) VALUES
(4, 28, 'Lechuga fresca', 0.45),
(4, 29, 'Cebolla nacional', 0.35),
(4, 30, 'Papa negra', 0.50),
(4, 27, 'Tomate', 0.70),
(4, 39, 'Arroz corriente', 0.80),
(4, 40, 'Harina multiusos', 0.85),
(4, 43, 'Aceite mezcla', 2.00),
(4, 44, 'Sal refinada', 0.30),
(4, 35, 'Coca Cola', 0.55),
(4, 55, 'Sprite', 0.55),
(4, 31, 'Cerveza Pilsener', 1.00),
(4, 56, 'Cerveza Corona', 1.20),
(4, 53, 'Galletas Oreo', 1.50),
(4, 54, 'Chocolate Nestlé', 1.80),
(4, 57, 'Detergente Ariel', 3.20),
(4, 58, 'Desinfectante Lysol', 3.80);

INSERT INTO inventory.company_skus (company_product_id, internal_sku, retail_price) VALUES
(56, 'DON-LECH-UND', 0.80),
(57, 'DON-CEBO-KG', 0.70),
(58, 'DON-PAPA-KG', 0.90),
(59, 'DON-TOMA-KG', 1.20),
(60, 'DON-ARRO-1KG', 1.30),
(61, 'DON-HARI-1KG', 1.40),
(62, 'DON-ACEI-1LT', 3.50),
(63, 'DON-SAL-1KG', 0.60),
(64, 'DON-COCA-500', 0.90),
(65, 'DON-SPRI-500', 0.90),
(66, 'DON-CERV-PIL', 1.80),
(67, 'DON-CERV-COR', 2.20),
(68, 'DON-GALL-ORE', 2.50),
(69, 'DON-CHOC-NES', 3.00),
(70, 'DON-DETE-ARI', 5.50),
(71, 'DON-DESI-LYS', 6.00);

INSERT INTO inventory.stocks (warehouse_id, sku_id, quantity, reserved_quantity) VALUES
(10, 72, 100, 0),(10, 73, 80, 0),(10, 74, 120, 0),(10, 75, 90, 0),
(10, 76, 200, 0),(10, 77, 150, 0),(10, 78, 100, 0),(10, 79, 180, 0),
(11, 72, 80, 0),(11, 73, 60, 0),(11, 74, 90, 0),(11, 75, 70, 0),
(11, 76, 150, 0),(11, 77, 120, 0),
(12, 80, 300, 0),(12, 81, 300, 0),
(12, 82, 200, 0),(12, 83, 150, 0),
(12, 84, 250, 0),(12, 85, 200, 0),
(12, 86, 100, 0),(12, 87, 80, 0);

INSERT INTO inventory.movements (company_id, warehouse_id, status_id, type_id, notes) VALUES
(4, 10, 2, 1, 'Compra inicial Almacén Central'),
(4, 11, 2, 1, 'Compra inicial Perecederos'),
(4, 12, 2, 1, 'Compra inicial Almacén Seco');

INSERT INTO inventory.movement_details (movement_id, sku_id, quantity, unit_cost) VALUES
(10, 72, 100, 0.45),(10, 73, 80, 0.35),(10, 74, 120, 0.50),(10, 75, 90, 0.70),
(10, 76, 200, 0.80),(10, 77, 150, 0.85),(10, 78, 100, 2.00),(10, 79, 180, 0.30),
(11, 72, 80, 0.45),(11, 73, 60, 0.35),(11, 74, 90, 0.50),(11, 75, 70, 0.70),
(11, 76, 150, 0.80),(11, 77, 120, 0.85),
(12, 80, 300, 0.55),(12, 81, 300, 0.55),
(12, 82, 200, 1.00),(12, 83, 150, 1.20),
(12, 84, 250, 1.50),(12, 85, 200, 1.80),
(12, 86, 100, 3.20),(12, 87, 80, 3.80);

INSERT INTO inventory.kardex (company_id, warehouse_id, sku_id, movement_detail_id, type_id, quantity, balance_after) VALUES
(4, 10, 72, 108, 1, 100, 100),(4, 10, 73, 109, 1, 80, 80),(4, 10, 74, 110, 1, 120, 120),(4, 10, 75, 111, 1, 90, 90),
(4, 10, 76, 112, 1, 200, 200),(4, 10, 77, 113, 1, 150, 150),(4, 10, 78, 114, 1, 100, 100),(4, 10, 79, 115, 1, 180, 180),
(4, 11, 72, 116, 1, 80, 80),(4, 11, 73, 117, 1, 60, 60),(4, 11, 74, 118, 1, 90, 90),(4, 11, 75, 119, 1, 70, 70),
(4, 11, 76, 120, 1, 150, 150),(4, 11, 77, 121, 1, 120, 120),
(4, 12, 80, 122, 1, 300, 300),(4, 12, 81, 123, 1, 300, 300),
(4, 12, 82, 124, 1, 200, 200),(4, 12, 83, 125, 1, 150, 150),
(4, 12, 84, 126, 1, 250, 250),(4, 12, 85, 127, 1, 200, 200),
(4, 12, 86, 128, 1, 100, 100),(4, 12, 87, 129, 1, 80, 80);

INSERT INTO sales.customers (company_id, name, phone, email) VALUES
(4, 'Rosa Mamani', '77804001', NULL),
(4, 'Tienda El Buen Precio', '77804002', 'tienda@buenprecio.com'),
(4, 'Restaurante La Familia', '77804003', 'pedidos@lafamilia.com');

INSERT INTO sales.sellers (company_id, name, phone) VALUES
(4, 'Don Carlos Quispe', '77814001'),
(4, 'Doña Carmen Quispe', '77814002');

INSERT INTO sales.sales (company_id, warehouse_id, seller_id, customer_id, status_id, notes) VALUES
(4, 10, 9, 10, 2, 'Venta mostrador Rosa Mamani'),
(4, 12, 10, 12, 2, 'Pedido Restaurante La Familia');

INSERT INTO sales.sale_details (sale_id, sku_id, quantity, unit_price) VALUES
(6, 72, 3, 0.80),(6, 73, 2, 0.70),(6, 76, 5, 1.30),(6, 80, 6, 0.90),
(7, 76, 20, 1.30),(7, 77, 10, 1.40),(7, 78, 15, 3.50),(7, 72, 10, 0.80);

INSERT INTO sales.receipts (sale_id, total_amount) VALUES
(6, 12.30),
(7, 71.50);
