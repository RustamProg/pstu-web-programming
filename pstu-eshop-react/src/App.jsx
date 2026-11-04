import { useState } from 'react'
import './App.css'

export default function App() {
  const [count, setCount] = useState(1)

  const addProduct = () => setCount(prev => prev + 1)

  return (
    <main>
      <h1>PSTU EShop React App</h1>

      <p>Товаров в корзине: {count}</p>

      <button onClick={addProduct}>
        Добавить товар
      </button>
    </main>
  )
}
