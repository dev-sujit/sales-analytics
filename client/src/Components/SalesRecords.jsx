import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import { saleRecords } from '../Constants/SalesRecord';

const SalesRecords = ({sendUrl}) => {
    const navigate = useNavigate();
    const [salesData, setSalesData] = useState()
    const [page, setpage] = useState(1)
    const [searchById, setSearchById] = useState()
    
    console.log("searchById", typeof searchById);

    const config = {
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` }
    };

    const handleEdit = (id) => {
        axios.get(`${sendUrl}/${id}`, config).then((res) => {
            var value = {data: res.data.data, updateUrl: `$`};
            navigate("/addsalerecord", {state: value})
        }).catch((error) => {
            console.log(error);
        })
        
      };
    
      const handleDelete = (id) => {
        axios.delete(`${sendUrl}/${id}`, config).then((res) => {
            getData()
        }).catch((error) => {
            console.log(error);
        })
      };

    const handlePrev = () => {
        if (page != 1) {
            setpage(page - 1)
        };
      };
    
      const handleNext = () => {
        setpage(page + 1)
      };

      const getData = () => {
        if (searchById == null || searchById == "") {
            axios.get(`${sendUrl}?page=${page}&pageSize=10`, config).then((res) => {
                setSalesData(res.data.data)
                console.log(res.data);
            }).catch((error) => {
                console.log(error);
            })
        } 

        axios.get(`${sendUrl}/${searchById}`, config).then((res) => {
            if (res.data.data != null) {
                var newArray = [];
                newArray.push(res.data.data)
                setSalesData(newArray)
            }
            console.log(res.data);
        }).catch((error) => {
            console.log(error);
        })
      }

      useEffect(() => {
        getData()
      }, [page, searchById])
      

  return (
    <div className="mx-auto px-4 py-8 px-4">
      <h2 className="text-xl font-semibold mb-4">Sales Records</h2>
      <input placeholder='search by id' className='py-3 px-2 mb-4' type='number' onChange={e => setSearchById(e.target.value)}/>
      <div className="overflow-x-auto">
        <table className="table-auto min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr className="text-left text-gray-700">
              <th className="px-6 py-3 text-sm font-semibold">ID</th>
              <th className="px-6 py-3 text-sm font-semibold">Date</th>
              <th className="px-6 py-3 text-sm font-semibold">Product</th>
              <th className="px-6 py-3 text-sm font-semibold">Quantity</th>
              <th className="px-6 py-3 text-sm font-semibold">Price</th>
              <th className="px-6 py-3 text-sm font-semibold">Number of Sales</th>
              <th className="px-6 py-3 text-sm font-semibold">Region</th>
              <th className="px-6 py-3 text-sm font-semibold">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {salesData != null ? salesData.map((sale) => (
              <tr key={sale.id} className="text-gray-700">
                <td className="px-6 py-4 whitespace-nowrap">{sale.id}</td>
                <td className="px-6 py-4 whitespace-nowrap">{new Date(sale.date).toLocaleString()}</td>
                <td className="px-6 py-4 whitespace-nowrap">{sale.product}</td>
                <td className="px-6 py-4 whitespace-nowrap">{sale.quantity}</td>
                <td className="px-6 py-4 whitespace-nowrap">${sale.price}</td>
                <td className="px-6 py-4 whitespace-nowrap">{sale.numberOfSales}</td>
                <td className="px-6 py-4 whitespace-nowrap">{sale.region}</td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <button
                    onClick={() => handleEdit(sale.id)}
                    className="text-blue-600 hover:underline mr-2"
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => handleDelete(sale.id)}
                    className="text-red-600 hover:underline"
                  >
                    Delete
                  </button>
                </td>
              </tr>
            )) : (
                <>Loading</>
            )}
          </tbody>
        </table>
      </div>
      <br/>
      <div>
        <button onClick={handlePrev} className="text-blue-600 hover:underline mr-2">Prev</button>
        <button onClick={handleNext} className="text-blue-600 hover:underline mr-2">Next</button>
      </div>
    </div>
  )
}

export default SalesRecords
