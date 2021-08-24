
using Dapper;
using DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class UserService
    {
        private string ConnectionString { get; set; }
        public UserService(string connectionString)
        {
            ConnectionString = connectionString;
        }

       

        public List<User> GetAllUsers()
        {
            List<User> userList = new List<User>();
            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = "USPGetAllUsers";
                connection.Open();
                var users = connection.Query(sql, commandType: System.Data.CommandType.StoredProcedure).AsList();

                users.ForEach(r =>
                    { 
                     User user = new User();

                        user.Id = Convert.ToInt32(r.Id);
                        user.FirstName = Convert.ToString(r.FirstName);
                        user.LastName = Convert.ToString(r.LastName);

                        State state = new State();

                        state.Id = Convert.ToInt32(r.StateId);
                        state.Name = Convert.ToString(r.StateName);

                        user.State = state;

                        userList.Add(user);
                }
                );

            }


          
            return userList;
        }
        public List<State> GetAllStates()
        {
            List<State> StateList = new List<State>();

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = "SELECT * FROM UDFGetAllStates()";
                connection.Open();
                var users = connection.Query(sql, commandType: System.Data.CommandType.Text).AsList();

                users.ForEach(r =>
                {
                   State state = new State();

                    state.Id = Convert.ToInt32(r.Id);
                    state.Name = Convert.ToString(r.Name);
                    StateList.Add(state);
                }
                );

            }
            return StateList;
        }

        public bool CreateUser(User model)
        {
            var returnVal = 0;

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = "USPCreateUser";
                connection.Open();
                var param = new DynamicParameters();

                param.Add("@FirstName", model.FirstName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                param.Add("@LastName", model.LastName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                param.Add("@StateId", model.State.Id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

                returnVal = connection.Execute(sql, param, commandType: System.Data.CommandType.StoredProcedure);

            }
            return returnVal > 0 ? true : false;
        }

        public User GetUser(int id)
        {
            User user = new User();

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = "USPGetUser";
                connection.Open();
                var param = new DynamicParameters();

                param.Add("@UserId", id, System.Data.DbType.String, System.Data.ParameterDirection.Input);
               

                var r = connection.QueryFirst(sql, param, commandType: System.Data.CommandType.StoredProcedure);

                user.Id = Convert.ToInt32(r.Id);
                user.FirstName = Convert.ToString(r.FirstName);
                user.LastName = Convert.ToString(r.LastName);

                State state = new State();

                state.Id = Convert.ToInt32(r.StateId);
                state.Name = Convert.ToString(r.StateName);

                user.State = state;

            }
           
            return user;
        }

        public bool UpdateUser(User model)
        {
            var returnVal = 0;

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = "USPUpdateUser";
                connection.Open();
                var param = new DynamicParameters();


                param.Add("@UserId", model.Id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input); 
                param.Add("@FirstName", model.FirstName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                param.Add("@LastName", model.LastName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                param.Add("@StateId", model.State.Id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

                returnVal = connection.Execute(sql, param, commandType: System.Data.CommandType.StoredProcedure);

            }
            return returnVal > 0 ? true : false;
        }

        public bool DeleteUser(int id)
        {
            var returnVal = 0;

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = "USPDeleteUser";
                connection.Open();
                var param = new DynamicParameters();


                param.Add("@UserId", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
               

                returnVal = connection.Execute(sql, param, commandType: System.Data.CommandType.StoredProcedure);

            }
            return returnVal > 0 ? true : false;
        }
    }
}
