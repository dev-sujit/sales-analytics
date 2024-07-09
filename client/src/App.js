import React from 'react'
import Sidebar from './Components/Sidebar'
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import FormComponent from './Components/FormComponent';
import { login, register } from './Constants/Auth';
import { saleRecords } from './Constants/SalesRecord';
import SalesRecords from './Components/SalesRecords';
import Dashboard from './Components/Dashboard';

const router = createBrowserRouter([
  {
    path: "/",
    element: <Sidebar/>,
    children: [
      {
        path: "",
        element: <Dashboard/>
      }
    ]
  },
  {
    path: "login",
    element: <Sidebar/>,
    children: [
      {
        path: "",
        element: <FormComponent data={login} buttonText="Login" sendUrl={`${process.env.REACT_APP_BASE_URL}/Authentication/login`} />
      }
    ]
  },
  {
    path: "register",
    element: <Sidebar/>,
    children: [
      {
        path: "",
        element: <FormComponent data={register} buttonText="Register" sendUrl={`${process.env.REACT_APP_BASE_URL}/Authentication/register`} />
      }
    ]
  },
  {
    path: "salerecords",
    element: <Sidebar/>,
    children: [
      {
        path: "",
        element: <SalesRecords sendUrl={`${process.env.REACT_APP_BASE_URL}/SaleRecords`}/>
      }
    ]
  },
  {
    path: "addsalerecord",
    element: <Sidebar/>,
    children: [
      {
        path: "",
        element: <FormComponent data={saleRecords} buttonText="Add Sale Record" sendUrl={`${process.env.REACT_APP_BASE_URL}/SaleRecords`} />
      }
    ]
  },
]);

const App = () => {
  return (
    <>
      <RouterProvider router={router} />
    </>
  )
}

export default App
