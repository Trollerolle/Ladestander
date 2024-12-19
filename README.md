# Ladestander
Bookingsystem til Sæther

Guide til opsætning af server og opstart af programmet: 
1.	Hav din egen lokale SQL-server klar og kørende…
2.	Åben Zip filen og lokaliser SQL mappen
3.	Kør de 5 SQL filer i rækkefølgen: 1-5 via din SSMS 

4.	Lokaliser din appsettings.json: 
4.1	  Gå ind i mappen El_Booking og derefter El_booking igen
4.2	  Åben med notesblok eller visual studio for at redigere
4.3	  Lokaliser følgende 3 steder i filen:
	  "Server=RENEXPS\\SQLEXPRESS;Database=El_Booking;"

4.4	  Fjern alt mellem "Server=" og ";Database=El_Booking;"
4.5	  Indsæt dit servernavn i stedet og gem filen
4.5a	  Hvis du har en enkelt '\' i dit servernavn skal det skrives som 2: '\\'

5.	Åben visual studio, build solution og start

