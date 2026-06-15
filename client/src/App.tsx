import { FormEvent, useEffect, useState } from "react";

type Order = {
  id: number;
  number: string;
  senderCity: string;
  senderAddress: string;
  receiverCity: string;
  receiverAddress: string;
  weightKg: number;
  pickupDate: string;
};

const fields = [
  ["senderCity", "Город отправителя", "text"],
  ["senderAddress", "Адрес отправителя", "text"],
  ["receiverCity", "Город получателя", "text"],
  ["receiverAddress", "Адрес получателя", "text"],
  ["weightKg", "Вес груза, кг", "number"],
  ["pickupDate", "Дата забора груза", "date"],
] as const;

export default function App() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [selected, setSelected] = useState<Order | null>(null);
  const [form, setForm] = useState<Record<string, string>>({});
  const [error, setError] = useState("");

  useEffect(() => {
    fetch("/api/orders")
      .then((response) => response.json())
      .then(setOrders)
      .catch(() => setError("Не удалось загрузить заказы."));
  }, []);

  async function createOrder(event: FormEvent) {
    event.preventDefault();
    const response = await fetch("/api/orders", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ ...form, weightKg: Number(form.weightKg) }),
    });

    if (!response.ok) {
      setError("Проверьте поля заказа и попробуйте снова.");
      return;
    }

    setOrders([await response.json(), ...orders]);
    setForm({});
    setError("");
  }

  async function openOrder(id: number) {
    const response = await fetch(`/api/orders/${id}`);
    if (response.ok) {
      setSelected(await response.json());
    }
  }

  if (selected) {
    return (
      <main>
        <h2>Заказ {selected.number}</h2>
        <dl>
          {fields.map(([name, label]) => (
            <div key={name}>
              <dt>{label}</dt>
              <dd>{String(selected[name])}</dd>
            </div>
          ))}
        </dl>
        <button onClick={() => setSelected(null)}>← К списку</button>
      </main>
    );
  }

  return (
    <main>
      <form onSubmit={createOrder}>
        <h1>Новый заказ</h1>
        {fields.map(([name, label, type]) => (
          <label key={name}>
            {label}
            <input
              required
              type={type}
              min={type === "number" ? "0.001" : undefined}
              step={type === "number" ? "0.001" : undefined}
              value={form[name] ?? ""}
              onChange={(event) => setForm({ ...form, [name]: event.target.value })}
            />
          </label>
        ))}
        {error && <p className="error">{error}</p>}
        <button>Создать заказ</button>
      </form>

      <h2>Заказы</h2>
      {orders.length === 0 && <p>Пока заказов нет</p>}
      {orders.map((order) => (
        <button key={order.id} className="order" onClick={() => openOrder(order.id)}>
          {order.number} — {order.senderCity} → {order.receiverCity}
        </button>
      ))}
    </main>
  );
}
