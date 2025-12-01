-- =============================================
-- Bus Booking System - PostgreSQL Database Setup
-- =============================================

-- Create Database (Run this separately first)
-- CREATE DATABASE "BusBookingDB";

-- Connect to the database and run the following:

-- =============================================
-- 1. CREATE TABLES
-- =============================================

-- Users Table
CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(100) NOT NULL UNIQUE,
    "PasswordHash" TEXT NOT NULL,
    "Phone" VARCHAR(15) NOT NULL,
    "Role" VARCHAR(20) NOT NULL DEFAULT 'User',
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

-- Buses Table
CREATE TABLE "Buses" (
    "Id" SERIAL PRIMARY KEY,
    "BusNumber" VARCHAR(50) NOT NULL UNIQUE,
    "BusType" VARCHAR(50) NOT NULL,
    "TotalSeats" INTEGER NOT NULL,
    "Amenities" VARCHAR(500),
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Routes Table
CREATE TABLE "Routes" (
    "Id" SERIAL PRIMARY KEY,
    "Origin" VARCHAR(100) NOT NULL,
    "Destination" VARCHAR(100) NOT NULL,
    "Distance" DECIMAL(10,2) NOT NULL,
    "Duration" INTEGER NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Schedules Table
CREATE TABLE "Schedules" (
    "Id" SERIAL PRIMARY KEY,
    "BusId" INTEGER NOT NULL,
    "RouteId" INTEGER NOT NULL,
    "DepartureTime" TIMESTAMP NOT NULL,
    "ArrivalTime" TIMESTAMP NOT NULL,
    "Price" DECIMAL(10,2) NOT NULL,
    "TravelDate" TIMESTAMP NOT NULL,
    "AvailableSeats" INTEGER NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_Schedules_Buses" FOREIGN KEY ("BusId") REFERENCES "Buses"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Schedules_Routes" FOREIGN KEY ("RouteId") REFERENCES "Routes"("Id") ON DELETE CASCADE
);

-- Bookings Table
CREATE TABLE "Bookings" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "ScheduleId" INTEGER NOT NULL,
    "BookingReference" VARCHAR(50) NOT NULL UNIQUE,
    "BookingDate" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "Status" VARCHAR(20) NOT NULL DEFAULT 'Pending',
    "TotalAmount" DECIMAL(10,2) NOT NULL,
    "NumberOfSeats" INTEGER NOT NULL,
    CONSTRAINT "FK_Bookings_Users" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Bookings_Schedules" FOREIGN KEY ("ScheduleId") REFERENCES "Schedules"("Id") ON DELETE CASCADE
);

-- Seats Table
CREATE TABLE "Seats" (
    "Id" SERIAL PRIMARY KEY,
    "BookingId" INTEGER NOT NULL,
    "SeatNumber" VARCHAR(10) NOT NULL,
    "PassengerName" VARCHAR(100) NOT NULL,
    "Age" INTEGER NOT NULL,
    "Gender" VARCHAR(10) NOT NULL,
    CONSTRAINT "FK_Seats_Bookings" FOREIGN KEY ("BookingId") REFERENCES "Bookings"("Id") ON DELETE CASCADE
);

-- Payments Table
CREATE TABLE "Payments" (
    "Id" SERIAL PRIMARY KEY,
    "BookingId" INTEGER NOT NULL UNIQUE,
    "Amount" DECIMAL(10,2) NOT NULL,
    "PaymentMethod" VARCHAR(50) NOT NULL,
    "TransactionId" VARCHAR(100),
    "Status" VARCHAR(20) NOT NULL DEFAULT 'Pending',
    "PaymentDate" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_Payments_Bookings" FOREIGN KEY ("BookingId") REFERENCES "Bookings"("Id") ON DELETE CASCADE
);

-- =============================================
-- 2. CREATE INDEXES
-- =============================================

CREATE INDEX "IX_Users_Email" ON "Users"("Email");
CREATE INDEX "IX_Buses_BusNumber" ON "Buses"("BusNumber");
CREATE INDEX "IX_Routes_Origin_Destination" ON "Routes"("Origin", "Destination");
CREATE INDEX "IX_Schedules_BusId" ON "Schedules"("BusId");
CREATE INDEX "IX_Schedules_RouteId" ON "Schedules"("RouteId");
CREATE INDEX "IX_Schedules_TravelDate" ON "Schedules"("TravelDate");
CREATE INDEX "IX_Bookings_UserId" ON "Bookings"("UserId");
CREATE INDEX "IX_Bookings_ScheduleId" ON "Bookings"("ScheduleId");
CREATE INDEX "IX_Bookings_BookingReference" ON "Bookings"("BookingReference");
CREATE INDEX "IX_Seats_BookingId" ON "Seats"("BookingId");
CREATE INDEX "IX_Payments_BookingId" ON "Payments"("BookingId");

-- =============================================
-- 3. INSERT SEED DATA
-- =============================================

-- Insert Admin User (password: Admin@123)
INSERT INTO "Users" ("Name", "Email", "PasswordHash", "Phone", "Role", "CreatedAt", "IsActive")
VALUES ('Admin User', 'admin@busbooking.com', '$2a$11$XqZQqH8rXvK5YvHX9kGYkOzJQ7J.vKJZqF4pGx5K9qO8YqJqKqKqK', '1234567890', 'Admin', CURRENT_TIMESTAMP, TRUE);

-- Insert Sample Routes
INSERT INTO "Routes" ("Origin", "Destination", "Distance", "Duration", "IsActive", "CreatedAt")
VALUES 
    ('New York', 'Boston', 215.00, 240, TRUE, CURRENT_TIMESTAMP),
    ('Los Angeles', 'San Francisco', 380.00, 360, TRUE, CURRENT_TIMESTAMP),
    ('Chicago', 'Detroit', 280.00, 300, TRUE, CURRENT_TIMESTAMP),
    ('Miami', 'Orlando', 235.00, 210, TRUE, CURRENT_TIMESTAMP),
    ('Seattle', 'Portland', 174.00, 180, TRUE, CURRENT_TIMESTAMP);

-- Insert Sample Buses
INSERT INTO "Buses" ("BusNumber", "BusType", "TotalSeats", "Amenities", "IsActive", "CreatedAt")
VALUES 
    ('BUS001', 'AC Sleeper', 40, 'WiFi, Charging, Water, Blanket', TRUE, CURRENT_TIMESTAMP),
    ('BUS002', 'Non-AC Seater', 50, 'Water', TRUE, CURRENT_TIMESTAMP),
    ('BUS003', 'AC Seater', 45, 'WiFi, Charging, Water', TRUE, CURRENT_TIMESTAMP),
    ('BUS004', 'Luxury AC Sleeper', 36, 'WiFi, Charging, Water, Blanket, Entertainment', TRUE, CURRENT_TIMESTAMP),
    ('BUS005', 'AC Semi-Sleeper', 42, 'WiFi, Charging, Water', TRUE, CURRENT_TIMESTAMP);

-- Insert Sample Schedules (for next 7 days)
INSERT INTO "Schedules" ("BusId", "RouteId", "DepartureTime", "ArrivalTime", "Price", "TravelDate", "AvailableSeats", "IsActive", "CreatedAt")
VALUES 
    -- Route 1: New York to Boston
    (1, 1, CURRENT_TIMESTAMP + INTERVAL '1 day' + INTERVAL '8 hours', CURRENT_TIMESTAMP + INTERVAL '1 day' + INTERVAL '12 hours', 45.00, CURRENT_TIMESTAMP + INTERVAL '1 day', 40, TRUE, CURRENT_TIMESTAMP),
    (3, 1, CURRENT_TIMESTAMP + INTERVAL '1 day' + INTERVAL '14 hours', CURRENT_TIMESTAMP + INTERVAL '1 day' + INTERVAL '18 hours', 40.00, CURRENT_TIMESTAMP + INTERVAL '1 day', 45, TRUE, CURRENT_TIMESTAMP),
    
    -- Route 2: Los Angeles to San Francisco
    (2, 2, CURRENT_TIMESTAMP + INTERVAL '2 days' + INTERVAL '9 hours', CURRENT_TIMESTAMP + INTERVAL '2 days' + INTERVAL '15 hours', 55.00, CURRENT_TIMESTAMP + INTERVAL '2 days', 50, TRUE, CURRENT_TIMESTAMP),
    (4, 2, CURRENT_TIMESTAMP + INTERVAL '2 days' + INTERVAL '20 hours', CURRENT_TIMESTAMP + INTERVAL '3 days' + INTERVAL '2 hours', 75.00, CURRENT_TIMESTAMP + INTERVAL '2 days', 36, TRUE, CURRENT_TIMESTAMP),
    
    -- Route 3: Chicago to Detroit
    (5, 3, CURRENT_TIMESTAMP + INTERVAL '3 days' + INTERVAL '7 hours', CURRENT_TIMESTAMP + INTERVAL '3 days' + INTERVAL '12 hours', 35.00, CURRENT_TIMESTAMP + INTERVAL '3 days', 42, TRUE, CURRENT_TIMESTAMP),
    
    -- Route 4: Miami to Orlando
    (1, 4, CURRENT_TIMESTAMP + INTERVAL '4 days' + INTERVAL '10 hours', CURRENT_TIMESTAMP + INTERVAL '4 days' + INTERVAL '13 hours 30 minutes', 30.00, CURRENT_TIMESTAMP + INTERVAL '4 days', 40, TRUE, CURRENT_TIMESTAMP),
    
    -- Route 5: Seattle to Portland
    (3, 5, CURRENT_TIMESTAMP + INTERVAL '5 days' + INTERVAL '15 hours', CURRENT_TIMESTAMP + INTERVAL '5 days' + INTERVAL '18 hours', 25.00, CURRENT_TIMESTAMP + INTERVAL '5 days', 45, TRUE, CURRENT_TIMESTAMP);

    
    
    Sele
    
    
-- =============================================
-- 4. VERIFICATION QUERIES
-- =============================================

-- Verify data insertion
SELECT 'Users' as TableName, COUNT(*) as RecordCount FROM "Users"
UNION ALL
SELECT 'Buses', COUNT(*) FROM "Buses"
UNION ALL
SELECT 'Routes', COUNT(*) FROM "Routes"
UNION ALL
SELECT 'Schedules', COUNT(*) FROM "Schedules";

-- View all schedules with bus and route details
SELECT 
    s."Id",
    b."BusNumber",
    b."BusType",
    r."Origin",
    r."Destination",
    s."DepartureTime",
    s."ArrivalTime",
    s."Price",
    s."AvailableSeats"
FROM "Schedules" s
JOIN "Buses" b ON s."BusId" = b."Id"
JOIN "Routes" r ON s."RouteId" = r."Id"
ORDER BY s."TravelDate", s."DepartureTime";






select * from "Schedules" s 
join "Buses" b on s."BusId" = b."Id"
join "Routes" r on r."Id" = s."RouteId" 















