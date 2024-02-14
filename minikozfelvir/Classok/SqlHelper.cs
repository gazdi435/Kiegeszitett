using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;

namespace minikozfelvir.Classok;

public static class SqlHelper
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
    private static readonly string table = "temp";

    public static string ConnectionString { get => connectionString; }

    public static async Task<List<Diak>> Import(this MySqlConnection conn)
    {
        List<Diak> contacts = new();
        MySqlDataReader? reader = default;

        try
        {
            await conn.OpenAsync();

            MySqlCommand command = new($"SELECT * FROM {table};", conn);
            reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                contacts.Add(new Diak(reader));
            }
        }
        finally
        {
            await reader.CloseAsync();
            conn.Close();
        }

        return contacts;
    }

    public static async Task Export(this MySqlConnection conn, Diak diak)
    {
        MySqlCommand cmd;
        await conn.OpenAsync();

        cmd = new($"INSERT INTO {table} (OM_Azonosito, Nev, Email, Ertesitesi_cim, Szuletesi_datum, Matek_eredmeny, Magyar_eredmeny) VALUES(@OM, @Nev, @Email, @Cim, @Datum, @Matek, @Magyar);", conn);

        cmd.Parameters.AddWithValue("@OM", diak.OM_Azonosito);
        cmd.Parameters.AddWithValue("@Nev", diak.Neve);
        cmd.Parameters.AddWithValue("@Email", diak.Email);
        cmd.Parameters.AddWithValue("@Cim", diak.ErtesitesiCime);
        cmd.Parameters.AddWithValue("@Datum", diak.SzuletesiDatum);
        cmd.Parameters.AddWithValue("@Matek", diak.Matematika);
        cmd.Parameters.AddWithValue("@Magyar", diak.Magyar);

        try
        {
            await cmd.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex)
        {
            // Handle specific exception (e.g., duplicate entry)
            if (ex.Number == 1062) // MySQL error code for duplicate entry
            {
                MessageBox.Show("Hiba! A sor nem lett hozzáadva, mert már volt ilyen azonosítóval.");
            }
            else
            {
                throw;
            }
        }

        conn.Close();
    }

    public static async Task Export(this MySqlConnection conn, IEnumerable<Diak> diakok)
    {
        MySqlCommand cmd;
        int hibasSorok = 0;
        await conn.OpenAsync();

        cmd = new($"INSERT INTO {table} (OM_Azonosito, Nev, Email, Ertesitesi_cim, Szuletesi_datum, Matek_eredmeny, Magyar_eredmeny) VALUES(@OM, @Nev, @Email, @Cim, @Datum, @Matek, @Magyar);", conn);

        cmd.Parameters.Add("@OM", MySqlDbType.VarChar);
        cmd.Parameters.Add("@Nev", MySqlDbType.VarChar);
        cmd.Parameters.Add("@Email", MySqlDbType.VarChar);
        cmd.Parameters.Add("@Cim", MySqlDbType.VarChar);
        cmd.Parameters.Add("@Datum", MySqlDbType.Date);
        cmd.Parameters.Add("@Matek", MySqlDbType.Byte);
        cmd.Parameters.Add("@Magyar", MySqlDbType.Byte);

        foreach (Diak diak in diakok)
        {
            try
            {
                cmd.Parameters["@OM"].Value = diak.OM_Azonosito;
                cmd.Parameters["@Nev"].Value = diak.Neve;
                cmd.Parameters["@Email"].Value = diak.Email;
                cmd.Parameters["@Cim"].Value = diak.ErtesitesiCime;
                cmd.Parameters["@Datum"].Value = diak.SzuletesiDatum;
                cmd.Parameters["@Matek"].Value = (byte)diak.Matematika;
                cmd.Parameters["@Magyar"].Value = (byte)diak.Magyar;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (MySqlException ex)
            {
                // Handle specific exception (e.g., duplicate entry)
                if (ex.Number == 1062) // MySQL error code for duplicate entry
                {
                    hibasSorok++;
                }
                else
                {
                    throw;
                }
            }
        }

        if (hibasSorok > 0)
            MessageBox.Show($"Hiba! {hibasSorok}db sor nem lett hozzáadva, mert {(hibasSorok > 1 ? "már voltak olyanok" : "már volt olyan")}.");

        conn.Close();
    }
}
