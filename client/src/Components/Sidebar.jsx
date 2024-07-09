import React from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';

const Sidebar = () => {
    const navigate = useNavigate();
    const handleLogout = () => {
        localStorage.removeItem("token");
        navigate("/login");
    }
  return (
    <div className="sidebar flex">
      <ul className="sidebar-nav bg-blue-950 flex flex-col h-screen w-1/5 p-2 gap-5 text-white">
        {/* Auth */}
        
        {/* saleRecords */}
        {localStorage.getItem("token") != null ?
        (
            <>
                <Link className='p-2 hover:bg-blue-800' to="/">Sales Dashboard</Link>
                <Link className='p-2 hover:bg-blue-800' to="/salerecords">Get Sale Records</Link>
                <Link className='p-2 hover:bg-blue-800' to="/addsalerecord">Add Sale Record</Link>

                <button className='bg-blue-500 py-2' onClick={handleLogout}>Logout</button>
            </>
        ) : (
            <>
                <Link className='p-2 hover:bg-blue-800' to="/login">Login</Link>
                <Link className='p-2 hover:bg-blue-800' to="/register">Register</Link>
            </>
        )}
      </ul>

      <div className='w-full'>
        <Outlet/>
      </div>
    </div>
  );
}

export default Sidebar;
