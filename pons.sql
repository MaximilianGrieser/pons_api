-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 27. Apr 2023 um 12:09
-- Server-Version: 10.4.28-MariaDB
-- PHP-Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
DROP DATABASE pons;
--
-- Datenbank: `pons`
--
CREATE DATABASE pons;
-- --------------------------------------------------------
USE pons;
--
-- Tabellenstruktur für Tabelle `arab`
--

CREATE TABLE `arab` (
  `id` int(11) NOT NULL,
  `header` text NOT NULL,
  `translation_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `hit`
--

CREATE TABLE `hit` (
  `id` int(11) NOT NULL,
  `type` text NOT NULL,
  `opendict` tinyint(1) NOT NULL,
  `rom_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `lang`
--

CREATE TABLE `lang` (
  `id` int(11) NOT NULL,
  `description` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `language`
--

CREATE TABLE `language` (
  `id` int(11) NOT NULL,
  `lang_id` int(11) NOT NULL,
  `hit_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `rom`
--

CREATE TABLE `rom` (
  `id` int(11) NOT NULL,
  `headword` text NOT NULL,
  `headword_full` text NOT NULL,
  `wordclass_id` int(11) DEFAULT NULL,
  `arab_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `translation`
--

CREATE TABLE `translation` (
  `id` int(11) NOT NULL,
  `source` text NOT NULL,
  `target` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `wordclass`
--

CREATE TABLE `wordclass` (
  `id` int(11) NOT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `arab`
--
ALTER TABLE `arab`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `hit`
--
ALTER TABLE `hit`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `lang`
--
ALTER TABLE `lang`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `language`
--
ALTER TABLE `language`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `rom`
--
ALTER TABLE `rom`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `translation`
--
ALTER TABLE `translation`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `wordclass`
--
ALTER TABLE `wordclass`
  ADD PRIMARY KEY (`id`);

  
--
-- AUTO_INCREMENT für Tabelle `arab`
--
ALTER TABLE `arab`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `hit`
--
ALTER TABLE `hit`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `lang`
--
ALTER TABLE `lang`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `language`
--
ALTER TABLE `language`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `rom`
--
ALTER TABLE `rom`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `translation`
--
ALTER TABLE `translation`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `wordclass`
--
ALTER TABLE `wordclass`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- Foreignkeys für die Tabelle `language`
--
ALTER TABLE `language`
  ADD FOREIGN KEY (`hit_id`) REFERENCES `hit`(`id`),
  ADD FOREIGN KEY (`lang_id`) REFERENCES `lang`(`id`);
--
-- Foreignkeys für die Tabelle `hit`
--
ALTER TABLE `hit`
  ADD FOREIGN KEY (`rom_id`) REFERENCES `rom`(`id`);
--
-- Foreignkeys für die Tabelle `rom`
--
ALTER TABLE `rom`
  ADD FOREIGN KEY (`arab_id`) REFERENCES `arab`(`id`),
  ADD FOREIGN KEY (`wordclass_id`) REFERENCES `wordclass`(`id`);
--
-- Foreignkeys für die Tabelle `arab`
--
ALTER TABLE `arab`
  ADD FOREIGN KEY (`translation_id`) REFERENCES `translation`(`id`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
