-- MySQL dump 10.13  Distrib 8.0.26, for Win64 (x86_64)
--
-- Host: localhost    Database: hci_domdb
-- ------------------------------------------------------
-- Server version	8.0.26

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `zaposleni`
--

DROP TABLE IF EXISTS `zaposleni`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zaposleni` (
  `OsobaID` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `Email` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `Sifra` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `DatumZaposlenja` date NOT NULL,
  `Paviljon` enum('Paviljon 1','Paviljon 2','Paviljon 3','Paviljon 4') COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`OsobaID`),
  UNIQUE KEY `Username` (`Username`),
  UNIQUE KEY `Email` (`Email`),
  CONSTRAINT `zaposleni_ibfk_1` FOREIGN KEY (`OsobaID`) REFERENCES `osoba` (`OsobaID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb3 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zaposleni`
--

LOCK TABLES `zaposleni` WRITE;
/*!40000 ALTER TABLE `zaposleni` DISABLE KEYS */;
INSERT INTO `zaposleni` VALUES (2,'mm','z1@gmail.com','97e93b0192c81365f9326c49834aab8062ce9a95c18cdbc095c34d19ece5276c','2001-01-11','Paviljon 1'),(5,'Ivan','ivan@gmail.com','fef4be63d80adef25b20de995b44a0fb73a45f70bb45c6d2410edb9de161670b','2025-01-10','Paviljon 3'),(9,'nemanja','n@gmail.com','d217efe5859854f7385ef62dedf0491627e9c8d6189aae9fe75523a9b44d6766','2025-01-02','Paviljon 2'),(12,'Rade','rade@gmail.com','ab23c5ec50cd893f6c1601ed90635a067cf1ac9a5a3e70d2f841a53653b27219','2024-12-13','Paviljon 1'),(59,'stole','sto@gmail.com','98979444d23f0955b6133915241bf26d96a1b981f16d4b0e94b36f93869bd513','2025-05-21','Paviljon 1'),(60,'tele','tel@gmail.com','2730b35d426effb945f1666a936de11466286266bfe172762eb2e00a4380c3af','2025-05-21','Paviljon 1'),(62,'tel','tel1@gmail.com','30a8a62e799dec9c199753b05dfb6dd07df04c1bf8eb9ab45b40027d4ce0fd4d','2025-05-29','Paviljon 4'),(69,'Šone','pepe@gmail.com','974a2be4c0f6db85c78778e367e905f6f4c1b3524505872ade3ddae1d9ef43b8','2025-05-14','Paviljon 1'),(70,'Dejo','dejan@gmail.com','3b3d9c42572b1da1ef9586e5ba5df7820d083acc630f1e6c7e1a15573f53db56','2025-06-26','Paviljon 1');
/*!40000 ALTER TABLE `zaposleni` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-05 11:10:22
