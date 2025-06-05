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
-- Table structure for table `osoba`
--

DROP TABLE IF EXISTS `osoba`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `osoba` (
  `OsobaID` int NOT NULL AUTO_INCREMENT,
  `Ime` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `Prezime` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `BrojTelefona` varchar(20) COLLATE utf8_unicode_ci DEFAULT NULL,
  `DatumRodjenja` date DEFAULT NULL,
  `AdresaStanovanja` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `JMBG` varchar(13) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Zvanje` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Tema` enum('Svijetla','Tamna','Zelena') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'Tamna',
  `Jezik` enum('Engleski','Srpski') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'Srpski',
  PRIMARY KEY (`OsobaID`),
  UNIQUE KEY `JMBG` (`JMBG`)
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb3 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `osoba`
--

LOCK TABLES `osoba` WRITE;
/*!40000 ALTER TABLE `osoba` DISABLE KEYS */;
INSERT INTO `osoba` VALUES (1,'Admin','Admin','066000000','2011-11-11','Banja Luka','1111111111111','Admin','Tamna','Engleski'),(2,'Marko','Markovic','066111111','2025-11-11','AdrZaposleni1','1111111111112','referent','Tamna','Srpski'),(5,'Ivan','Ivanovic','066666777','2025-01-10','Nema','1709995180868','Student','Svijetla','Engleski'),(9,'Nemanja','Novic','065666777','2025-01-09','Derventa','1709995111111','Referent za smještaj','Tamna','Engleski'),(12,'Radenko','Lukic','066007531','1995-09-17','Doboj, Dobojskih brigada 77','1709995180858','Direktor','Tamna','Srpski'),(59,'stole','stolic','053123432','2025-05-07','Banja Luka, Motike 12','1234323456548','stolar','Tamna','Srpski'),(60,'tel','efon','099546324','2025-05-07','nema ','6544564321234','phone','Tamna','Srpski'),(62,'asdasd','aaaaaaaa','325423453','2025-05-20','nema je trenutno','1235432346543','123123','Tamna','Srpski'),(69,'Nebojša','Ivančevic','065/222-222','2025-05-29','BL','9875675432345','Referent za isranu','Zelena','Srpski'),(70,'Dejan','Dejanović','099774374','2025-06-27','Bulevar vojvode Petra Bojovića, Banja Luka','7639857366583','Upravnik','Tamna','Srpski');
/*!40000 ALTER TABLE `osoba` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-05 11:10:21
