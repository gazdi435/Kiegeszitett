-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2024. Feb 04. 16:21
-- Kiszolgáló verziója: 10.4.28-MariaDB
-- PHP verzió: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `minikozfelvir`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `felvetelizok`
--

CREATE TABLE `felvetelizok` (
  `OM_Azonosito` varchar(11) NOT NULL,
  `Nev` varchar(40) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Szuletesi_datum` date DEFAULT NULL,
  `Ertesitesi_cim` varchar(120) DEFAULT NULL,
  `Matek_eredmeny` tinyint(4) DEFAULT NULL,
  `Magyar_eredmeny` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `felvetelizok`
--

INSERT INTO `felvetelizok` (`OM_Azonosito`, `Nev`, `Email`, `Szuletesi_datum`, `Ertesitesi_cim`, `Matek_eredmeny`, `Magyar_eredmeny`) VALUES
('01234567890', 'Teszt Jázmin', 'teszt.jazmin@example.com', '2003-01-24', 'Kaposvár, Berényi út 8.', 42, 17),
('12345678901', 'Teszt Andor', 'teszt.andor@example.com', '1994-08-20', 'Szeged, Kossuth tér 2.', 5, 19),
('12345678902', 'Teszt Elek', 'teszt.elek@example.com', '1994-08-20', 'Szeged, Kossuth tér 2.', 14, 19),
('23456789012', 'Teszt Borbála', 'teszt.borbala@example.com', '1995-09-12', 'Budapest, Deák tér 5.', NULL, NULL),
('23456789013', 'Teszt Mária', 'teszt.maria@example.com', '1995-09-12', 'Budapest, Deák tér 5.', 2, 5),
('32575612875', 'Teszt Viktória', 'teszt.viktoria@example.com', '2003-01-24', 'Kaposvár, Berényi út 8.', 33, 28),
('34567890123', 'Teszt Csaba', 'teszt.csaba@example.com', '1996-11-03', 'Debrecen, Kossuth utca 1.', NULL, NULL),
('34567890124', 'Teszt Gábor', 'teszt.gabor@example.com', '1996-11-03', 'Debrecen, Kossuth utca 1.', NULL, NULL),
('45678901234', 'Teszt Dóra', 'teszt.dora@example.com', '1997-02-18', 'Pécs, Szent István tér 3.', 28, 35),
('45678901235', 'Teszt Zsuzsa', 'teszt.zsuzsa@example.com', '1997-02-18', 'Pécs, Szent István tér 3.', 20, 33),
('56789012345', 'Teszt Endre', 'teszt.endre@example.com', '1998-04-25', 'Győr, Baross utca 8.', 37, 33),
('56789012346', 'Teszt István', 'teszt.istvan@example.com', '1998-04-25', 'Győr, Baross utca 8.', 3, 18),
('67890123456', 'Teszt Franciska', 'teszt.franciska@example.com', '1999-06-10', 'Miskolc, Bajnai út 14.', 1, 8),
('67890123457', 'Teszt Katalin', 'teszt.katalin@example.com', '1999-06-10', 'Miskolc, Bajnai út 14.', 33, 10),
('78901234567', 'Teszt Gergő', 'teszt.gergo@example.com', '2000-08-30', 'Szombathely, Ady Endre út 7.', 38, 13),
('78901234568', 'Teszt József', 'teszt.jozsef@example.com', '2000-08-30', 'Szombathely, Ady Endre út 7.', 2, 31),
('89012345678', 'Teszt Hanna', 'teszt.hanna@example.com', '2001-10-14', 'Eger, Dózsa György út 9.', NULL, NULL),
('89012345679', 'Teszt Anna', 'teszt.anna@example.com', '2001-10-14', 'Eger, Dózsa György út 9.', NULL, NULL),
('90123456780', 'Teszt László', 'teszt.laszlo@example.com', '2002-12-01', 'Nyíregyháza, Petőfi tér 5.', 48, 45),
('90123456789', 'Teszt Imre', 'teszt.imre@example.com', '2002-12-01', 'Nyíregyháza, Petőfi tér 5.', 4, 31);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `felvetelizok`
--
ALTER TABLE `felvetelizok`
  ADD PRIMARY KEY (`OM_Azonosito`),
  ADD KEY `idx_Nev` (`Nev`),
  ADD KEY `idx_OM_Azonosito` (`OM_Azonosito`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
