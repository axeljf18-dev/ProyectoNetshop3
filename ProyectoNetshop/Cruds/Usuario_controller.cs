﻿using Microsoft.Data.SqlClient;
using ProyectoNetshop.BD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNetshop.Cruds
{
    internal class Usuario_controller
    {
        public static int agregarUsuario(Usuario_model p_usuario)
        {
            using var conexion = BD.BaseDeDatos.obtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"INSERT INTO usuario (nombre, apellido, email, pass, activo, sexo, fecha_nacimiento, telefono, dni, id_perfil)
                                VALUES (@nombre, @apellido, @email, @pass, @activo, @sexo, @fecha_nacimiento, @telefono, @dni, @id_perfil);";

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = p_usuario.nombre;
            cmd.Parameters.Add("@apellido", SqlDbType.VarChar, 100).Value = p_usuario.apellido;
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 200).Value = p_usuario.email;
            cmd.Parameters.Add("@pass", SqlDbType.VarBinary, 256).Value = p_usuario.pass ?? Array.Empty<byte>();
            cmd.Parameters.Add("@activo", SqlDbType.Int).Value = p_usuario.activo;
            cmd.Parameters.Add("@sexo", SqlDbType.VarChar, 20).Value = p_usuario.sexo;
            cmd.Parameters.Add("@fecha_nacimiento", SqlDbType.Date).Value = p_usuario.fecha_nacimiento.HasValue ? (object)p_usuario.fecha_nacimiento.Value.Date : DBNull.Value;
            
            cmd.Parameters.Add("@telefono",SqlDbType.BigInt).Value = p_usuario.telefono.HasValue ? (object)p_usuario.telefono.Value : DBNull.Value;

            cmd.Parameters.Add("@dni", SqlDbType.Int).Value = p_usuario.dni;
            cmd.Parameters.Add("@id_perfil", SqlDbType.Int).Value = p_usuario.id_perfil;

            return cmd.ExecuteNonQuery();
        }

        public static int actualizarUsuario(Usuario_model u)
        {
            using var conexion = BD.BaseDeDatos.obtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"UPDATE usuario SET nombre = @nombre, apellido = @apellido, email = @email, pass = @pass, activo = @activo, sexo = @sexo, 
                                                    fecha_nacimiento = @fecha_nacimiento, telefono = @telefono, dni = @dni, 
                                                    id_perfil = @id_perfil WHERE id_usuario = @id_usuario;";
            cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = u.nombre;
            cmd.Parameters.Add("@apellido", SqlDbType.VarChar, 100).Value = u.apellido;
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 200).Value = u.email;
            cmd.Parameters.Add("@pass", SqlDbType.VarBinary, 256).Value = u.pass ?? Array.Empty<byte>();
            cmd.Parameters.Add("@activo", SqlDbType.Int).Value = u.activo;
            cmd.Parameters.Add("@sexo", SqlDbType.VarChar, 20).Value = u.sexo;

            var pFecha = cmd.Parameters.Add("@fecha_nacimiento", SqlDbType.Date);
            pFecha.Value = u.fecha_nacimiento.HasValue ? (object)u.fecha_nacimiento.Value.Date : DBNull.Value;

            var pTel = cmd.Parameters.Add("@telefono", SqlDbType.BigInt);
            pTel.Value = u.telefono.HasValue ? (object)u.telefono.Value : DBNull.Value;

            cmd.Parameters.Add("@dni", SqlDbType.Int).Value = u.dni;
            cmd.Parameters.Add("@id_perfil", SqlDbType.Int).Value = u.id_perfil;
            cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = u.id_usuario;

            return cmd.ExecuteNonQuery();
        }

        public static int eliminarUsuario(int idUsuario)
        {
            using var con = BD.BaseDeDatos.obtenerConexion();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"UPDATE usuario SET activo = 0 WHERE id_usuario = @idUsuario AND activo = 1;";
            cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;

            return cmd.ExecuteNonQuery();
        }


        public static List<Usuario_model> ObtenerVendedores()
        {
            var lista = new List<Usuario_model>();

            using var conexion = BaseDeDatos.obtenerConexion();
            using var cmd = new SqlCommand(@"SELECT dni, nombre, apellido FROM usuario WHERE id_perfil = 2 AND activo = 1;", conexion);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Usuario_model
                {
                    dni = reader.GetInt32(reader.GetOrdinal("dni")),
                    nombre = reader.GetString(reader.GetOrdinal("nombre")),
                    apellido = reader.GetString(reader.GetOrdinal("apellido"))
                });
            }

            return lista;
        }

        public static bool ExisteEmailODni(int dni, string email, int idUsuario = 0)
        {
            using var conexion = BD.BaseDeDatos.obtenerConexion();
            using var cmd = conexion.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM usuario WHERE (dni = @dni OR email = @email) AND (@idUsuario = 0 OR id_usuario <> @idUsuario);";
            cmd.Parameters.Add("@dni", SqlDbType.Int).Value = dni;
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 200).Value = email;
            cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }
    }
}
