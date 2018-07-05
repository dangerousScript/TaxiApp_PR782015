﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaxiApp.Models.Classes;
using System.IO;

namespace TaxiApp.Controllers
{
    public class KorisnikController : ApiController
    {
		// edit user info
		// PUT api/Korisnik/1
		public bool Put(int id, [FromBody]Korisnik korisnik)
		{
			if (korisnik.Uloga == Uloge.Musterija)
			{
				foreach (Korisnik kor in Korisnici.korisnici.Values)
				{
					if (kor.Id == id)
					{
						Korisnici.korisnici.Remove(kor.Id);
						Korisnici.korisnici.Add(korisnik.Id, korisnik);
						UpisIzmenaTxt(korisnik);
						return true;
					}
				}
				return false;
			}
			else if (korisnik.Uloga == Uloge.Dispecer)
			{
				foreach (Dispecer kor in Dispeceri.dispeceri.Values)
				{
					if (kor.Id == id)
					{
						Dispeceri.dispeceri.Remove(kor.Id);
						Dispecer d = new Dispecer(korisnik.Id, korisnik.KorisnickoIme, korisnik.Lozinka, korisnik.Ime, korisnik.Prezime, korisnik.Pol, korisnik.JMBG, korisnik.KontaktTelefon, korisnik.Email, korisnik.Uloga);
						Dispeceri.dispeceri.Add(d.Id, d);
						UpisIzmenaDispTxt(korisnik);
						return true;
					}
				}
				return false;
			}
			else
			{

				return false;
			}

		}

		private void UpisIzmenaTxt(Korisnik k)
		{
			string[] lines = File.ReadAllLines(@"C:\Users\stefan\Desktop\FAX\Web\TaxiApp_PR782015\TaxiApp\TaxiApp\App_Data\Korisnici.txt");
			string allString = "";
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].Contains(k.Id.ToString()))
				{
					allString += k.Id.ToString() + '|' + k.KorisnickoIme + '|' + k.Lozinka + '|' + k.Ime + '|' + k.Prezime + '|' + k.Pol + '|' + k.JMBG + '|' + k.KontaktTelefon + '|' + k.Email + '|' + k.Uloga;
					lines[i] = allString;
				}
			}
			File.WriteAllLines(@"C:\Users\stefan\Desktop\FAX\Web\TaxiApp_PR782015\TaxiApp\TaxiApp\App_Data\Korisnici.txt", lines);

		}

		private void UpisIzmenaDispTxt(Korisnik k)
		{
			string[] lines = File.ReadAllLines(@"C:\Users\stefan\Desktop\FAX\Web\TaxiApp_PR782015\TaxiApp\TaxiApp\App_Data\Dispeceri.txt");
			string allString = "";
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].Contains(k.Id.ToString()))
				{
					allString += k.Id.ToString() + '|' + k.KorisnickoIme + '|' + k.Lozinka + '|' + k.Ime + '|' + k.Prezime + '|' + k.Pol + '|' + k.JMBG + '|' + k.KontaktTelefon + '|' + k.Email + '|' + k.Uloga;
					lines[i] = allString;
				}
			}
			File.WriteAllLines(@"C:\Users\stefan\Desktop\FAX\Web\TaxiApp_PR782015\TaxiApp\TaxiApp\App_Data\Dispeceri.txt", lines);
		}

		// POST api/korisnik
		public bool Post([FromBody]Korisnik korisnik)
		{

			foreach (Korisnik kor in Korisnici.korisnici.Values)
			{
				if (kor.KorisnickoIme == korisnik.KorisnickoIme)
				{
					return false;
				}
			}

			string[] idCount = File.ReadAllLines(@"C:\Users\stefan\Desktop\FAX\Web\TaxiApp_PR782015\TaxiApp\TaxiApp\App_Data\Korisnici.txt");

			Korisnici.korisnici = new Dictionary<int, Korisnik>();
			korisnik.Id = idCount.Length + 1;
			korisnik.Uloga = Uloge.Musterija;
			Korisnici.korisnici.Add(korisnik.Id, korisnik);
			UpisTxt(korisnik);
			return true;
		}

		private void UpisTxt(Korisnik k)
		{
			FileStream stream = new FileStream(@"C:\Users\stefan\Desktop\FAX\Web\TaxiApp_PR782015\TaxiApp\TaxiApp\App_Data\Korisnici.txt", FileMode.Append);
			using (StreamWriter tw = new StreamWriter(stream))
			{
				string upis = k.Id.ToString() + '|' + k.KorisnickoIme + '|' + k.Lozinka + '|' + k.Ime + '|' + k.Prezime + '|' + k.Pol + '|' + k.JMBG + '|' + k.KontaktTelefon + '|' + k.Email + '|' + k.Uloga;
				tw.WriteLine(upis);
			}
			stream.Close();
		}
	}
}
